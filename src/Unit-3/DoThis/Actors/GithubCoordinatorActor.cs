// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GithubCoordinatorActor.cs" company="">
//   
// </copyright>
// <summary>
//   Actor responsible for publishing data about the results
//   of a github operation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GithubCoordinatorActor.cs" company="none">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace GithubActors.Actors
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Akka.Actor;
    using Akka.Routing;

    using Octokit;

    #endregion

    /// <summary>
    ///     Actor responsible for publishing data about the results
    ///     of a github operation
    /// </summary>
    public class GithubCoordinatorActor : ReceiveActor
    {
        /// <summary>
        ///     The _current repo.
        /// </summary>
        private RepoKey currentRepo;

        /// <summary>
        ///     The _github progress stats.
        /// </summary>
        private GithubProgressStats githubProgressStats;

        /// <summary>
        ///     The _github worker.
        /// </summary>
        private IActorRef githubWorker;

        /// <summary>
        ///     The _publish timer.
        /// </summary>
        private ICancelable publishTimer;

        /// <summary>
        ///     The _received initial users.
        /// </summary>
        private bool receivedInitialUsers;

        /// <summary>
        ///     The _similar repos.
        /// </summary>
        private Dictionary<string, SimilarRepo> similarRepos;

        /// <summary>
        ///     The _subscribers.
        /// </summary>
        private HashSet<IActorRef> subscribers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GithubCoordinatorActor" /> class.
        /// </summary>
        public GithubCoordinatorActor()
        {
            this.Waiting();
        }

        /// <summary>
        ///     The pre start.
        /// </summary>
        protected override void PreStart()
        {
            this.githubWorker =
                Context.ActorOf(
                    Props.Create(() => new GithubWorkerActor(GithubClientFactory.GetClient))
                        .WithRouter(new RoundRobinPool(10)));
        }

        /// <summary>
        ///     The waiting.
        /// </summary>
        private void Waiting()
        {
            this.Receive<GithubCommanderActor.CanAcceptJob>(
                job => this.Sender.Tell(new GithubCommanderActor.AbleToAcceptJob(job.Repo)));
            this.Receive<BeginJob>(
                job =>
                    {
                        this.BecomeWorking(job.Repo);

                        // kick off the job to query the repo's list of starrers
                        this.githubWorker.Tell(new RetryableQuery(new GithubWorkerActor.QueryStarrers(job.Repo), 4));
                    });
        }

        /// <summary>
        /// The become working.
        /// </summary>
        /// <param name="repo">
        /// The repo.
        /// </param>
        private void BecomeWorking(RepoKey repo)
        {
            this.receivedInitialUsers = false;
            this.currentRepo = repo;
            this.subscribers = new HashSet<IActorRef>();
            this.similarRepos = new Dictionary<string, SimilarRepo>();
            this.publishTimer = new Cancelable(Context.System.Scheduler);
            this.githubProgressStats = new GithubProgressStats();
            this.Become(this.Working);
        }

        /// <summary>
        ///     The become waiting.
        /// </summary>
        private void BecomeWaiting()
        {
            // stop publishing
            this.publishTimer.Cancel();
            this.Become(this.Waiting);
        }

        /// <summary>
        ///     The working.
        /// </summary>
        private void Working()
        {
            // received a downloaded user back from the github worker
            this.Receive<GithubWorkerActor.StarredReposForUser>(
                user =>
                    {
                        this.githubProgressStats = this.githubProgressStats.UserQueriesFinished();
                        foreach (var repo in user.Repos)
                        {
                            if (!this.similarRepos.ContainsKey(repo.HtmlUrl))
                            {
                                this.similarRepos[repo.HtmlUrl] = new SimilarRepo(repo);
                            }

                            // increment the number of people who starred this repo
                            this.similarRepos[repo.HtmlUrl].SharedStarrers++;
                        }
                    });

            this.Receive<PublishUpdate>(
                update =>
                    {
                        // check to see if the job is done
                        if (this.receivedInitialUsers && this.githubProgressStats.IsFinished)
                        {
                            this.githubProgressStats = this.githubProgressStats.Finish();

                            // all repos minus forks of the current one
                            var sortedSimilarRepos =
                                this.similarRepos.Values.Where(x => x.Repo.Name != this.currentRepo.Repo)
                                    .OrderByDescending(x => x.SharedStarrers)
                                    .ToList();
                            foreach (var subscriber in this.subscribers)
                            {
                                subscriber.Tell(sortedSimilarRepos);
                            }

                            this.BecomeWaiting();
                        }

                        foreach (var subscriber in this.subscribers)
                        {
                            subscriber.Tell(this.githubProgressStats);
                        }
                    });

            // completed our initial job - we now know how many users we need to query
            this.Receive<User[]>(
                users =>
                    {
                        this.receivedInitialUsers = true;
                        this.githubProgressStats = this.githubProgressStats.SetExpectedUserCount(users.Length);

                        // queue up all of the jobs
                        foreach (var user in users)
                        {
                            this.githubWorker.Tell(
                                new RetryableQuery(new GithubWorkerActor.QueryStarrer(user.Login), 3));
                        }
                    });

            this.Receive<GithubCommanderActor.CanAcceptJob>(
                job => this.Sender.Tell(new GithubCommanderActor.UnableToAcceptJob(job.Repo)));

            this.Receive<SubscribeToProgressUpdates>(
                updates =>
                    {
                        // this is our first subscriber, which means we need to turn publishing on
                        if (this.subscribers.Count == 0)
                        {
                            Context.System.Scheduler.ScheduleTellRepeatedly(
                                TimeSpan.FromMilliseconds(100), 
                                TimeSpan.FromMilliseconds(100), 
                                this.Self, 
                                PublishUpdate.Instance, 
                                this.Self, 
                                this.publishTimer);
                        }

                        this.subscribers.Add(updates.Subscriber);
                    });

            // query failed, but can be retried
            this.Receive<RetryableQuery>(query => query.CanRetry, query => this.githubWorker.Tell(query));

            // query failed, can't be retried, and it's a QueryStarrers operation - means the entire job failed
            this.Receive<RetryableQuery>(
                query => !query.CanRetry && query.Query is GithubWorkerActor.QueryStarrers, 
                query =>
                    {
                        this.receivedInitialUsers = true;
                        foreach (var subscriber in this.subscribers)
                        {
                            subscriber.Tell(new JobFailed(this.currentRepo));
                        }

                        this.BecomeWaiting();
                    });

            // query failed, can't be retried, and it's a QueryStarrers operation - means individual operation failed
            this.Receive<RetryableQuery>(
                query => !query.CanRetry && query.Query is GithubWorkerActor.QueryStarrer, 
                query => this.githubProgressStats.IncrementFailures());
        }

        #region Message classes

        /// <summary>
        ///     The begin job.
        /// </summary>
        public class BeginJob
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BeginJob"/> class.
            /// </summary>
            /// <param name="repo">
            /// The repo.
            /// </param>
            public BeginJob(RepoKey repo)
            {
                this.Repo = repo;
            }

            /// <summary>
            ///     Gets the repo.
            /// </summary>
            public RepoKey Repo { get; private set; }
        }

        /// <summary>
        ///     The subscribe to progress updates.
        /// </summary>
        public class SubscribeToProgressUpdates
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SubscribeToProgressUpdates"/> class.
            /// </summary>
            /// <param name="subscriber">
            /// The subscriber.
            /// </param>
            public SubscribeToProgressUpdates(IActorRef subscriber)
            {
                this.Subscriber = subscriber;
            }

            /// <summary>
            ///     Gets the subscriber.
            /// </summary>
            public IActorRef Subscriber { get; private set; }
        }

        /// <summary>
        ///     The publish update.
        /// </summary>
        public class PublishUpdate
        {
            /// <summary>
            ///     The _instance.
            /// </summary>
            private static readonly PublishUpdate _instance = new PublishUpdate();

            /// <summary>
            ///     Prevents a default instance of the <see cref="PublishUpdate" /> class from being created.
            /// </summary>
            private PublishUpdate()
            {
            }

            /// <summary>
            ///     Gets the instance.
            /// </summary>
            public static PublishUpdate Instance
            {
                get
                {
                    return _instance;
                }
            }
        }

        /// <summary>
        ///     Let the subscribers know we failed
        /// </summary>
        public class JobFailed
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="JobFailed"/> class.
            /// </summary>
            /// <param name="repo">
            /// The repo.
            /// </param>
            public JobFailed(RepoKey repo)
            {
                this.Repo = repo;
            }

            /// <summary>
            ///     Gets the repo.
            /// </summary>
            public RepoKey Repo { get; private set; }
        }

        #endregion
    }
}