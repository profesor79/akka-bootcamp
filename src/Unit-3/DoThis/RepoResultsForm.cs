#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="RepoResultsForm.cs" company="none">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace GithubActors
{
    #region Usings

    using System;
    using System.Windows.Forms;

    using Akka.Actor;

    using GithubActors.Actors;

    #endregion

    /// <summary>
    /// The repo results form.
    /// </summary>
    public partial class RepoResultsForm : Form
    {
        /// <summary>
        /// The _github coordinator.
        /// </summary>
        private readonly IActorRef _githubCoordinator;

        /// <summary>
        /// The _repo.
        /// </summary>
        private readonly RepoKey _repo;

        /// <summary>
        /// The _form actor.
        /// </summary>
        private IActorRef _formActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepoResultsForm"/> class.
        /// </summary>
        /// <param name="githubCoordinator">
        /// The github coordinator.
        /// </param>
        /// <param name="repo">
        /// The repo.
        /// </param>
        public RepoResultsForm(IActorRef githubCoordinator, RepoKey repo)
        {
            this._githubCoordinator = githubCoordinator;
            this._repo = repo;
            this.InitializeComponent();
        }

        /// <summary>
        /// The repo results form_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RepoResultsForm_Load(object sender, EventArgs e)
        {
            this._formActor =
                Program.GithubActors.ActorOf(
                    Props.Create(() => new RepoResultsActor(this.dgUsers, this.tsStatus, this.tsProgress))
                        .WithDispatcher("akka.actor.synchronized-dispatcher")); // run on the UI thread

            this.Text = string.Format("Repos Similar to {0} / {1}", this._repo.Owner, this._repo.Repo);

            // start subscribing to updates
            this._githubCoordinator.Tell(new GithubCoordinatorActor.SubscribeToProgressUpdates(this._formActor));
        }

        /// <summary>
        /// The repo results form_ form closing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RepoResultsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // kill the form actor
            this._formActor.Tell(PoisonPill.Instance);
        }
    }
}