#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GithubWorkerActor.cs" company="none">
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
    using System.Collections.Generic;
    using System.Linq;

    using Akka.Actor;

    using Octokit;

    #endregion

    /// <summary>
    ///     Individual actor responsible for querying the Github API
    /// </summary>
    public class GithubWorkerActor : ReceiveActor
    {
        /// <summary>
        /// The _git hub client factory.
        /// </summary>
        private readonly Func<IGitHubClient> _gitHubClientFactory;

        /// <summary>
        /// The _git hub client.
        /// </summary>
        private IGitHubClient _gitHubClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubWorkerActor"/> class.
        /// </summary>
        /// <param name="gitHubClientFactory">
        /// The git hub client factory.
        /// </param>
        public GithubWorkerActor(Func<IGitHubClient> gitHubClientFactory)
        {
            this._gitHubClientFactory = gitHubClientFactory;
            this.InitialReceives();
        }

        /// <summary>
        /// The pre start.
        /// </summary>
        protected override void PreStart()
        {
            this._gitHubClient = this._gitHubClientFactory();
        }

        /// <summary>
        /// The initial receives.
        /// </summary>
        private void InitialReceives()
        {
            // query an individual starrer
            this.Receive<RetryableQuery>(
                query => query.Query is QueryStarrer, 
                query =>
                    {
                        // ReSharper disable once PossibleNullReferenceException (we know from the previous IS statement that this is not null)
                        var starrer = (query.Query as QueryStarrer).Login;
                        try
                        {
                            var getStarrer = this._gitHubClient.Activity.Starring.GetAllForUser(starrer);

                            // ewww
                            getStarrer.Wait();
                            var starredRepos = getStarrer.Result;
                            this.Sender.Tell(new StarredReposForUser(starrer, starredRepos));
                        }
                        catch (Exception ex)
                        {
                            // operation failed - let the parent know
                            this.Sender.Tell(query.NextTry());
                        }
                    });

            // query all starrers for a repository
            this.Receive<RetryableQuery>(
                query => query.Query is QueryStarrers, 
                query =>
                    {
                        // ReSharper disable once PossibleNullReferenceException (we know from the previous IS statement that this is not null)
                        var starrers = (query.Query as QueryStarrers).Key;
                        try
                        {
                            var getStars = this._gitHubClient.Activity.Starring.GetAllStargazers(
                                starrers.Owner, 
                                starrers.Repo);

                            // ewww
                            getStars.Wait();
                            var stars = getStars.Result;
                            this.Sender.Tell(stars.ToArray());
                        }
                        catch (Exception ex)
                        {
                            // operation failed - let the parent know
                            this.Sender.Tell(query.NextTry());
                        }
                    });
        }

        #region Message classes

        /// <summary>
        /// The query starrers.
        /// </summary>
        public class QueryStarrers
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="QueryStarrers"/> class.
            /// </summary>
            /// <param name="key">
            /// The key.
            /// </param>
            public QueryStarrers(RepoKey key)
            {
                this.Key = key;
            }

            /// <summary>
            /// Gets the key.
            /// </summary>
            public RepoKey Key { get; private set; }
        }

        /// <summary>
        ///     Query an individual starrer
        /// </summary>
        public class QueryStarrer
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="QueryStarrer"/> class.
            /// </summary>
            /// <param name="login">
            /// The login.
            /// </param>
            public QueryStarrer(string login)
            {
                this.Login = login;
            }

            /// <summary>
            /// Gets the login.
            /// </summary>
            public string Login { get; private set; }
        }

        /// <summary>
        /// The starred repos for user.
        /// </summary>
        public class StarredReposForUser
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="StarredReposForUser"/> class.
            /// </summary>
            /// <param name="login">
            /// The login.
            /// </param>
            /// <param name="repos">
            /// The repos.
            /// </param>
            public StarredReposForUser(string login, IEnumerable<Repository> repos)
            {
                this.Repos = repos;
                this.Login = login;
            }

            /// <summary>
            /// Gets the login.
            /// </summary>
            public string Login { get; private set; }

            /// <summary>
            /// Gets the repos.
            /// </summary>
            public IEnumerable<Repository> Repos { get; private set; }
        }

        #endregion
    }
}