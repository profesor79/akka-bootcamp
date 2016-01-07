// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GithubCommanderActor.cs" company="">
//   
// </copyright>
// <summary>
//   Top-level actor responsible for coordinating and launching repo-processing jobs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GithubCommanderActor.cs" company="none">
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
    using System.Linq;

    using Akka.Actor;
    using Akka.Routing;

    #endregion

    /// <summary>
    ///     Top-level actor responsible for coordinating and launching repo-processing jobs
    /// </summary>
    public class GithubCommanderActor : ReceiveActor, IWithUnboundedStash
    {
        /// <summary>
        ///     The _can accept job sender.
        /// </summary>
        private IActorRef canAcceptJobSender;

        /// <summary>
        ///     The _coordinator.
        /// </summary>
        private IActorRef coordinator;

        /// <summary>
        /// The repo job.
        /// </summary>
        private RepoKey repoJob;

        /// <summary>
        ///     The pending job replies.
        /// </summary>
        private int pendingJobReplies;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GithubCommanderActor" /> class.
        /// </summary>
        public GithubCommanderActor()
        {
            this.Ready();
        }

        #region IWithUnboundedStash Members

        /// <summary>
        ///     Gets or sets the stash.
        /// </summary>
        public IStash Stash { get; set; }

        #endregion

        /// <summary>
        ///     The ready.
        /// </summary>
        private void Ready()
        {
            this.Receive<CanAcceptJob>(job =>
            {
                this.coordinator.Tell(job);
                this.repoJob = job.Repo;
                this.BecomeAsking();
            });
        }

        /// <summary>
        ///     The become asking.
        /// </summary>
        private void BecomeAsking()
        {
            this.canAcceptJobSender = this.Sender;

            // block, but ask the router for the number of routees. Avoids magic numbers.
            this.pendingJobReplies = this.coordinator.Ask<Routees>(new GetRoutees()).Result.Members.Count();
            this.Become(this.Asking);
            
            // send ourselves a ReceiveTimeout message if no message within 3 seonds
            Context.SetReceiveTimeout(TimeSpan.FromSeconds(3));
        }

        /// <summary>
        ///     The asking.
        /// </summary>
        private void Asking()
        {
            // stash any subsequent requests
            this.Receive<CanAcceptJob>(job => this.Stash.Stash());

            this.Receive<UnableToAcceptJob>(
                job =>
                    {
                        this.pendingJobReplies--;
                        if (this.pendingJobReplies == 0)
                        {
                            this.canAcceptJobSender.Tell(job);
                            this.BecomeReady();
                        }
                    });

            this.Receive<AbleToAcceptJob>(
                job =>
                    {
                        this.canAcceptJobSender.Tell(job);

                        // start processing messages
                        this.Sender.Tell(new GithubCoordinatorActor.BeginJob(job.Repo));

                        // launch the new window to view results of the processing
                        Context.ActorSelection(ActorPaths.MainFormActor.Path)
                            .Tell(new MainFormActor.LaunchRepoResultsWindow(job.Repo, this.Sender));

                        this.BecomeReady();
                    });

            // means at least one actor failed to respond
            this.Receive<ReceiveTimeout>(timeout =>
            {
                this.canAcceptJobSender.Tell(new UnableToAcceptJob(this.repoJob));
                this.BecomeReady();
            });
        }

        /// <summary>
        ///     The become ready.
        /// </summary>
        private void BecomeReady()
        {
            this.Become(this.Ready);
            this.Stash.UnstashAll();

            // cancel ReceiveTimeout
            Context.SetReceiveTimeout(null);
        }

        /// <summary>
        ///     The pre start.
        /// </summary>
        protected override void PreStart()
        {
            // create a broadcast router who will ask all of them if they're available for work
            this.coordinator =
                Context.ActorOf(
                    Props.Create(() => new GithubCoordinatorActor()).WithRouter(FromConfig.Instance), 
                    ActorPaths.GithubCoordinatorActor.Name);
            base.PreStart();
        }

        /// <summary>
        /// The pre restart.
        /// </summary>
        /// <param name="reason">
        /// The reason.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void PreRestart(Exception reason, object message)
        {
            // kill off the old coordinator so we can recreate it from scratch
            this.coordinator.Tell(PoisonPill.Instance);
            base.PreRestart(reason, message);
        }

        #region Message classes

        /// <summary>
        ///     The can accept job.
        /// </summary>
        public class CanAcceptJob
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CanAcceptJob"/> class.
            /// </summary>
            /// <param name="repo">
            /// The repo.
            /// </param>
            public CanAcceptJob(RepoKey repo)
            {
                this.Repo = repo;
            }

            /// <summary>
            ///     Gets the repo.
            /// </summary>
            public RepoKey Repo { get; private set; }
        }

        /// <summary>
        ///     The able to accept job.
        /// </summary>
        public class AbleToAcceptJob
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AbleToAcceptJob"/> class.
            /// </summary>
            /// <param name="repo">
            /// The repo.
            /// </param>
            public AbleToAcceptJob(RepoKey repo)
            {
                this.Repo = repo;
            }

            /// <summary>
            ///     Gets the repo.
            /// </summary>
            public RepoKey Repo { get; private set; }
        }

        /// <summary>
        ///     The unable to accept job.
        /// </summary>
        public class UnableToAcceptJob
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="UnableToAcceptJob"/> class.
            /// </summary>
            /// <param name="repo">
            /// The repo.
            /// </param>
            public UnableToAcceptJob(RepoKey repo)
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