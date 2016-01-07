#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GithubAuth.cs" company="none">
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
    using System.Diagnostics;
    using System.Windows.Forms;

    using Akka.Actor;

    using GithubActors.Actors;

    #endregion

    /// <summary>
    /// The github auth.
    /// </summary>
    public partial class GithubAuth : Form
    {
        /// <summary>
        /// The _auth actor.
        /// </summary>
        private IActorRef _authActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubAuth"/> class.
        /// </summary>
        public GithubAuth()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The github auth_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void GithubAuth_Load(object sender, EventArgs e)
        {
            this.linkGhLabel.Links.Add(
                new LinkLabel.Link
                    {
                        LinkData =
                            "https://help.github.com/articles/creating-an-access-token-for-command-line-use/"
                    });
            this._authActor =
                Program.GithubActors.ActorOf(
                    Props.Create(() => new GithubAuthenticationActor(this.lblAuthStatus, this)), 
                    ActorPaths.GithubAuthenticatorActor.Name);
        }

        /// <summary>
        /// The link gh label_ link clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void linkGhLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var link = e.Link.LinkData as string;
            if (link != null)
            {
                // Send the URL to the operating system via windows shell
                Process.Start(link);
            }
        }

        /// <summary>
        /// The btn authenticate_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            this._authActor.Tell(new GithubAuthenticationActor.Authenticate(this.tbOAuth.Text));
        }
    }
}