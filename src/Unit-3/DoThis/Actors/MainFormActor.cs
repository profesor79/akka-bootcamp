// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainFormActor.cs" company="">
//   
// </copyright>
// <summary>
//   Actor that runs on the UI thread and handles
//   UI events for
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MainFormActor.cs" company="none">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace GithubActors.Actors
{
    #region Usings

    using System.Drawing;
    using System.Windows.Forms;

    using Akka.Actor;

    #endregion

    /// <summary>
    ///     Actor that runs on the UI thread and handles
    ///     UI events for <see cref="LauncherForm" />
    /// </summary>
    public class MainFormActor : ReceiveActor, IWithUnboundedStash
    {
        /// <summary>
        ///     The _validation label.
        /// </summary>
        private readonly Label validationLabel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainFormActor"/> class.
        /// </summary>
        /// <param name="validationLabel">
        /// The validation label.
        /// </param>
        public MainFormActor(Label validationLabel)
        {
            this.validationLabel = validationLabel;
            this.Ready();
        }

        #region IWithUnboundedStash Members

        /// <summary>
        ///     Gets or sets the stash.
        /// </summary>
        public IStash Stash { get; set; }

        #endregion

        /// <summary>
        ///     State for when we're able to accept new jobs
        /// </summary>
        private void Ready()
        {
            this.Receive<ProcessRepo>(
                repo =>
                    {
                        Context.ActorSelection(ActorPaths.GithubValidatorActor.Path)
                            .Tell(new GithubValidatorActor.ValidateRepo(repo.RepoUri));
                        this.BecomeBusy(repo.RepoUri);
                    });

            // launch the window
            this.Receive<LaunchRepoResultsWindow>(
                window =>
                    {
                        var form = new RepoResultsForm(window.Coordinator, window.Repo);
                        form.Show();
                    });
        }

        /// <summary>
        /// Make any necessary URI updates, then switch our state to busy
        /// </summary>
        /// <param name="repoUrl">
        /// The repo Url.
        /// </param>
        private void BecomeBusy(string repoUrl)
        {
            this.validationLabel.Visible = true;
            this.validationLabel.Text = string.Format("Validating {0}...", repoUrl);
            this.validationLabel.ForeColor = Color.Gold;
            this.Become(this.Busy);
        }

        /// <summary>
        ///     State for when we're currently processing a job
        /// </summary>
        private void Busy()
        {
            this.Receive<GithubValidatorActor.RepoIsValid>(valid => this.BecomeReady("Valid!"));
            this.Receive<GithubValidatorActor.InvalidRepo>(invalid => this.BecomeReady(invalid.Reason, false));

            // yes
            this.Receive<GithubCommanderActor.UnableToAcceptJob>(
                job =>
                this.BecomeReady(
                    string.Format(
                        "{0}/{1} is a valid repo, but system can't accept additional jobs", 
                        job.Repo.Owner, 
                        job.Repo.Repo), 
                    false));

            // no
            this.Receive<GithubCommanderActor.AbleToAcceptJob>(
                job =>
                this.BecomeReady(
                    string.Format("{0}/{1} is a valid repo - starting job!", job.Repo.Owner, job.Repo.Repo)));
            this.Receive<LaunchRepoResultsWindow>(window => this.Stash.Stash());
        }

        /// <summary>
        /// The become ready.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="isValid">
        /// The is valid.
        /// </param>
        private void BecomeReady(string message, bool isValid = true)
        {
            this.validationLabel.Text = message;
            this.validationLabel.ForeColor = isValid ? Color.Green : Color.Red;
            this.Stash.UnstashAll();
            this.Become(this.Ready);
        }

        #region Nested type: LaunchRepoResultsWindow

        #region Messages

        /// <summary>
        ///     The launch repo results window.
        /// </summary>
        public class LaunchRepoResultsWindow
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="LaunchRepoResultsWindow"/> class.
            /// </summary>
            /// <param name="repo">
            /// The repo.
            /// </param>
            /// <param name="coordinator">
            /// The coordinator.
            /// </param>
            public LaunchRepoResultsWindow(RepoKey repo, IActorRef coordinator)
            {
                this.Repo = repo;
                this.Coordinator = coordinator;
            }

            /// <summary>
            ///     Gets the repo.
            /// </summary>
            public RepoKey Repo { get; private set; }

            /// <summary>
            ///     Gets the coordinator.
            /// </summary>
            public IActorRef Coordinator { get; private set; }
        }

        #endregion

        #endregion
    }
}