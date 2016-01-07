// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GithubClientFactory.cs" company="">
//   
// </copyright>
// <summary>
//   Creates  instances.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GithubClientFactory.cs" company="none">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace GithubActors
{
    #region Usings

    using Octokit;
    using Octokit.Internal;

    #endregion

    /// <summary>
    ///     Creates <see cref="GitHubClient" /> instances.
    /// </summary>
    public static class GithubClientFactory
    {
        /// <summary>
        ///     OAuth token - necessary to generate authenticated requests
        ///     and achieve non-terrible hourly API rate limit
        /// </summary>
        public static string OAuthToken { get; set; }

        /// <summary>
        ///     The get unauthenticated client.
        /// </summary>
        /// <returns>
        ///     The <see cref="GitHubClient" />.
        /// </returns>
        public static GitHubClient GetUnauthenticatedClient()
        {
            return new GitHubClient(new ProductHeaderValue("AkkaBootcamp-Unit3"));
        }

        /// <summary>
        ///     The get client.
        /// </summary>
        /// <returns>
        ///     The <see cref="GitHubClient" />.
        /// </returns>
        public static GitHubClient GetClient()
        {
            return new GitHubClient(
                new ProductHeaderValue("AkkaBootcamp-Unit3"), 
                new InMemoryCredentialStore(new Credentials(OAuthToken)));
        }
    }
}