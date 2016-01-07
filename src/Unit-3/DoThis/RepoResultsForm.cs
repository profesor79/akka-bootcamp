// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepoResultsForm.cs" company="">
//   
// </copyright>
// <summary>
//   The repo results form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    ///     The repo results form.
    /// </summary>
    public partial class RepoResultsForm : Form
    {
        /// <summary>
        ///     The _github coordinator.
        /// </summary>
        private readonly IActorRef githubCoordinator;

        /// <summary>
        ///     The _repo.
        /// </summary>
        private readonly RepoKey repo;

        /// <summary>
        ///     The _form actor.
        /// </summary>
        private IActorRef formActor;

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
            this.githubCoordinator = githubCoordinator;
            this.repo = repo;
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
        private void RepoResultsFormLoad(object sender, EventArgs e)
        {
            this.formActor =
                Program.GithubActors.ActorOf(
                    Props.Create(() => new RepoResultsActor(this.dgUsers, this.tsStatus, this.tsProgress))
                        .WithDispatcher("akka.actor.synchronized-dispatcher")); // run on the UI thread

            this.Text = string.Format("Repos Similar to {0} / {1}", this.repo.Owner, this.repo.Repo);

            // start subscribing to updates
            this.githubCoordinator.Tell(new GithubCoordinatorActor.SubscribeToProgressUpdates(this.formActor));
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
        private void RepoResultsFormFormClosing(object sender, FormClosingEventArgs e)
        {
            // kill the form actor
            this.formActor.Tell(PoisonPill.Instance);
        }
    }
}