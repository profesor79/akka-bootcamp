#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GithubAuthenticationActor.cs" company="none">
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

    using Akka.Actor;

    using Octokit;

    using Label = System.Windows.Forms.Label;

    #endregion

    /// <summary>
    /// The github authentication actor.
    /// </summary>
    public class GithubAuthenticationActor : ReceiveActor
    {
        /// <summary>
        /// The _form.
        /// </summary>
        private readonly GithubAuth _form;

        /// <summary>
        /// The _status label.
        /// </summary>
        private readonly Label _statusLabel;

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubAuthenticationActor"/> class.
        /// </summary>
        /// <param name="statusLabel">
        /// The status label.
        /// </param>
        /// <param name="form">
        /// The form.
        /// </param>
        public GithubAuthenticationActor(Label statusLabel, GithubAuth form)
        {
            this._statusLabel = statusLabel;
            this._form = form;
            this.Unauthenticated();
        }

        /// <summary>
        /// The unauthenticated.
        /// </summary>
        private void Unauthenticated()
        {
            this.Receive<Authenticate>(
                auth =>
                    {
                        // need a client to test our credentials with
                        var client = GithubClientFactory.GetUnauthenticatedClient();
                        GithubClientFactory.OAuthToken = auth.OAuthToken;
                        client.Credentials = new Credentials(auth.OAuthToken);
                        this.BecomeAuthenticating();
                        client.User.Current().ContinueWith<object>(
                            tr =>
                                {
                                    if (tr.IsFaulted)
                                    {
                                        return new AuthenticationFailed();
                                    }

                                    if (tr.IsCanceled)
                                    {
                                        return new AuthenticationCancelled();
                                    }

                                    return new AuthenticationSuccess();
                                }).PipeTo(this.Self);
                    });
        }

        /// <summary>
        /// The become authenticating.
        /// </summary>
        private void BecomeAuthenticating()
        {
            this._statusLabel.Visible = true;
            this._statusLabel.ForeColor = Color.Yellow;
            this._statusLabel.Text = "Authenticating...";
            this.Become(this.Authenticating);
        }

        /// <summary>
        /// The become unauthenticated.
        /// </summary>
        /// <param name="reason">
        /// The reason.
        /// </param>
        private void BecomeUnauthenticated(string reason)
        {
            this._statusLabel.ForeColor = Color.Red;
            this._statusLabel.Text = "Authentication failed. Please try again.";
            this.Become(this.Unauthenticated);
        }

        /// <summary>
        /// The authenticating.
        /// </summary>
        private void Authenticating()
        {
            this.Receive<AuthenticationFailed>(failed => this.BecomeUnauthenticated("Authentication failed."));
            this.Receive<AuthenticationCancelled>(cancelled => this.BecomeUnauthenticated("Authentication timed out."));
            this.Receive<AuthenticationSuccess>(
                success =>
                    {
                        var launcherForm = new LauncherForm();
                        launcherForm.Show();
                        this._form.Hide();
                    });
        }

        #region Messages

        /// <summary>
        /// The authenticate.
        /// </summary>
        public class Authenticate
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Authenticate"/> class.
            /// </summary>
            /// <param name="oAuthToken">
            /// The o auth token.
            /// </param>
            public Authenticate(string oAuthToken)
            {
                this.OAuthToken = oAuthToken;
            }

            /// <summary>
            /// Gets the o auth token.
            /// </summary>
            public string OAuthToken { get; private set; }
        }

        /// <summary>
        /// The authentication failed.
        /// </summary>
        public class AuthenticationFailed
        {
        }

        /// <summary>
        /// The authentication cancelled.
        /// </summary>
        public class AuthenticationCancelled
        {
        }

        /// <summary>
        /// The authentication success.
        /// </summary>
        public class AuthenticationSuccess
        {
        }

        #endregion
    }
}