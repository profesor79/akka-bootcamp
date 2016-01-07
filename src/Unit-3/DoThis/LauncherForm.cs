// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LauncherForm.cs" company="">
//   
// </copyright>
// <summary>
//   The launcher form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LauncherForm.cs" company="none">
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
    ///     The launcher form.
    /// </summary>
    public partial class LauncherForm : Form
    {
        /// <summary>
        ///     The _main form actor.
        /// </summary>
        private IActorRef mainFormActor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LauncherForm" /> class.
        /// </summary>
        public LauncherForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The main form_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MainFormLoad(object sender, EventArgs e)
        {
            /* INITIALIZE ACTORS */
            this.mainFormActor = Program.GithubActors.ActorOf(
                Props.Create(() => new MainFormActor(this.lblIsValid)), 
                ActorPaths.MainFormActor.Name);
            Program.GithubActors.ActorOf(
                Props.Create(() => new GithubValidatorActor(GithubClientFactory.GetClient())), 
                ActorPaths.GithubValidatorActor.Name);
            Program.GithubActors.ActorOf(
                Props.Create(() => new GithubCommanderActor()), 
                ActorPaths.GithubCommanderActor.Name);
        }

        /// <summary>
        /// The btn launch_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnLaunchClick(object sender, EventArgs e)
        {
            this.mainFormActor.Tell(new ProcessRepo(this.tbRepoUrl.Text));
        }

        /// <summary>
        /// The launcher form_ form closing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LauncherFormFormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}