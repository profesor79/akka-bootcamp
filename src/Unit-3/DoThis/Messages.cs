// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Messages.cs" company="">
//   
// </copyright>
// <summary>
//   Begin processing a new Github repository for analysis
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Messages.cs" company="none">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace GithubActors
{
    /// <summary>
    ///     Begin processing a new Github repository for analysis
    /// </summary>
    public class ProcessRepo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessRepo"/> class.
        /// </summary>
        /// <param name="repoUri">
        /// The repo uri.
        /// </param>
        public ProcessRepo(string repoUri)
        {
            this.RepoUri = repoUri;
        }

        /// <summary>
        ///     Gets the repo uri.
        /// </summary>
        public string RepoUri { get; private set; }
    }

    /// <summary>
    ///     The repo key.
    /// </summary>
    public class RepoKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepoKey"/> class.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        /// <param name="repo">
        /// The repo.
        /// </param>
        public RepoKey(string owner, string repo)
        {
            this.Repo = repo;
            this.Owner = owner;
        }

        /// <summary>
        ///     Gets the owner.
        /// </summary>
        public string Owner { get; private set; }

        /// <summary>
        ///     Gets the repo.
        /// </summary>
        public string Repo { get; private set; }
    }

    /// <summary>
    ///     The retryable query.
    /// </summary>
    public class RetryableQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RetryableQuery"/> class.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="allowableTries">
        /// The allowable tries.
        /// </param>
        public RetryableQuery(object query, int allowableTries)
            : this(query, allowableTries, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryableQuery"/> class.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="allowableTries">
        /// The allowable tries.
        /// </param>
        /// <param name="currentAttempt">
        /// The current attempt.
        /// </param>
        private RetryableQuery(object query, int allowableTries, int currentAttempt)
        {
            this.AllowableTries = allowableTries;
            this.Query = query;
            this.CurrentAttempt = currentAttempt;
        }

        /// <summary>
        ///     Gets the query.
        /// </summary>
        public object Query { get; private set; }

        /// <summary>
        ///     Gets the allowable tries.
        /// </summary>
        public int AllowableTries { get; private set; }

        /// <summary>
        ///     Gets the current attempt.
        /// </summary>
        public int CurrentAttempt { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether can retry.
        /// </summary>
        public bool CanRetry
        {
            get
            {
                return this.RemainingTries > 0;
            }
        }

        /// <summary>
        ///     Gets the remaining tries.
        /// </summary>
        public int RemainingTries
        {
            get
            {
                return this.AllowableTries - this.CurrentAttempt;
            }
        }

        /// <summary>
        ///     The next try.
        /// </summary>
        /// <returns>
        ///     The <see cref="RetryableQuery" />.
        /// </returns>
        public RetryableQuery NextTry()
        {
            return new RetryableQuery(this.Query, this.AllowableTries, this.CurrentAttempt + 1);
        }
    }
}