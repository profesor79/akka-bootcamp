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

    using Akka.Actor;
    using Akka.Routing;

    #endregion

    /// <summary>
    ///     Top-level actor responsible for coordinating and launching repo-processing jobs
    /// </summary>
    public class GithubCommanderActor : ReceiveActor, IWithUnboundedStash
    {
        /// <summary>
        /// The _can accept job sender.
        /// </summary>
        private IActorRef _canAcceptJobSender;

        /// <summary>
        /// The _coordinator.
        /// </summary>
        private IActorRef _coordinator;

        /// <summary>
        /// The pending job replies.
        /// </summary>
        private int pendingJobReplies;

        /// <summary>
        /// Gets or sets the stash.
        /// </summary>
        public IStash Stash { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubCommanderActor"/> class.
        /// </summary>
        public GithubCommanderActor()
        {
            this.Ready();
        }

        /// <summary>
        /// The ready.
        /// </summary>
        private void Ready()
        {
            this.Receive<CanAcceptJob>(job =>
            {
                this._coordinator.Tell(job);

                this.BecomeAsking();
            });
        }

        /// <summary>
        /// The become asking.
        /// </summary>
        private void BecomeAsking()
        {
            this._canAcceptJobSender = this.Sender;
            this.pendingJobReplies = 9; // the number of routees
            this.Become(this.Asking);
        }

        /// <summary>
        /// The asking.
        /// </summary>
        private void Asking()
        {
            // stash any subsequent requests
            this.Receive<CanAcceptJob>(job => this.Stash.Stash());

            this.Receive<UnableToAcceptJob>(job =>
            {
                this.pendingJobReplies--;
                if (this.pendingJobReplies == 0)
                {
                    this._canAcceptJobSender.Tell(job);
                    this.BecomeReady();
                }
            });

            this.Receive<AbleToAcceptJob>(job =>
            {
                this._canAcceptJobSender.Tell(job);

                // start processing messages
                this.Sender.Tell(new GithubCoordinatorActor.BeginJob(job.Repo));

                // launch the new window to view results of the processing
                Context.ActorSelection(ActorPaths.MainFormActor.Path).Tell(
                    new MainFormActor.LaunchRepoResultsWindow(job.Repo, this.Sender));

                this.BecomeReady();
            });
        }

        /// <summary>
        /// The become ready.
        /// </summary>
        private void BecomeReady()
        {
            this.Become(this.Ready);
            this.Stash.UnstashAll();
        }


        /// <summary>
        /// The pre start.
        /// </summary>
        protected override void PreStart()
        {
            // create three GithubCoordinatorActor instances
            Context.ActorOf(Props.Create(() => new GithubCoordinatorActor()), ActorPaths.GithubCoordinatorActor.Name + "1");
            Context.ActorOf(Props.Create(() => new GithubCoordinatorActor()), ActorPaths.GithubCoordinatorActor.Name + "2");
            Context.ActorOf(Props.Create(() => new GithubCoordinatorActor()), ActorPaths.GithubCoordinatorActor.Name + "3");

            // create a broadcast router who will ask all of them if they're available for work
            this._coordinator =
                Context.ActorOf(
                    Props.Empty.WithRouter(
                        new BroadcastGroup(
                            ActorPaths.GithubCoordinatorActor.Path + "1",
                            ActorPaths.GithubCoordinatorActor.Path + "2",
                            ActorPaths.GithubCoordinatorActor.Path + "3")));
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
            this._coordinator.Tell(PoisonPill.Instance);
            base.PreRestart(reason, message);
        }

        #region Message classes

        /// <summary>
        /// The can accept job.
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
            /// Gets the repo.
            /// </summary>
            public RepoKey Repo { get; private set; }
        }

        /// <summary>
        /// The able to accept job.
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
            /// Gets the repo.
            /// </summary>
            public RepoKey Repo { get; private set; }
        }

        /// <summary>
        /// The unable to accept job.
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
            /// Gets the repo.
            /// </summary>
            public RepoKey Repo { get; private set; }
        }

        #endregion
    }
}