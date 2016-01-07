// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GithubAuth.cs" company="">
//   
// </copyright>
// <summary>
//   The github auth.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    ///     The github auth.
    /// </summary>
    public partial class GithubAuth : Form
    {
        /// <summary>
        ///     The _auth actor.
        /// </summary>
        private IActorRef authActor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GithubAuth" /> class.
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
        private void GithubAuthLoad(object sender, EventArgs e)
        {
            this.linkGhLabel.Links.Add(
                new LinkLabel.Link
                    {
                        LinkData =
                            "https://help.github.com/articles/creating-an-access-token-for-command-line-use/"
                    });
            this.authActor =
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
        private void LinkGhLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
        private void BtnAuthenticateClick(object sender, EventArgs e)
        {
            this.authActor.Tell(new GithubAuthenticationActor.Authenticate(this.tbOAuth.Text));
        }
    }
}