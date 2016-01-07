// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GithubProgressStats.cs" company="">
//   
// </copyright>
// <summary>
//   used to sort the list of similar repos
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GithubProgressStats.cs" company="none">
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

    using Octokit;

    #endregion

    /// <summary>
    ///     used to sort the list of similar repos
    /// </summary>
    public class SimilarRepo : IComparable<SimilarRepo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimilarRepo"/> class.
        /// </summary>
        /// <param name="repo">
        /// The repo.
        /// </param>
        public SimilarRepo(Repository repo)
        {
            this.Repo = repo;
        }

        /// <summary>
        ///     Gets the repo.
        /// </summary>
        public Repository Repo { get; private set; }

        /// <summary>
        ///     Gets or sets the shared starrers.
        /// </summary>
        public int SharedStarrers { get; set; }

        #region IComparable<SimilarRepo> Members

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int CompareTo(SimilarRepo other)
        {
            return this.SharedStarrers.CompareTo(other.SharedStarrers);
        }

        #endregion
    }

    /// <summary>
    ///     Used to report on incremental progress.
    ///     Immutable.
    /// </summary>
    public class GithubProgressStats
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GithubProgressStats" /> class.
        /// </summary>
        public GithubProgressStats()
        {
            this.StartTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubProgressStats"/> class.
        /// </summary>
        /// <param name="startTime">
        /// The start time.
        /// </param>
        /// <param name="expectedUsers">
        /// The expected users.
        /// </param>
        /// <param name="usersThusFar">
        /// The users thus far.
        /// </param>
        /// <param name="queryFailures">
        /// The query failures.
        /// </param>
        /// <param name="endTime">
        /// The end time.
        /// </param>
        private GithubProgressStats(
            DateTime startTime, 
            int expectedUsers, 
            int usersThusFar, 
            int queryFailures, 
            DateTime? endTime)
        {
            this.EndTime = endTime;
            this.QueryFailures = queryFailures;
            this.UsersThusFar = usersThusFar;
            this.ExpectedUsers = expectedUsers;
            this.StartTime = startTime;
        }

        /// <summary>
        ///     Gets the expected users.
        /// </summary>
        public int ExpectedUsers { get; private set; }

        /// <summary>
        ///     Gets the users thus far.
        /// </summary>
        public int UsersThusFar { get; private set; }

        /// <summary>
        ///     Gets the query failures.
        /// </summary>
        public int QueryFailures { get; private set; }

        /// <summary>
        ///     Gets the start time.
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        ///     Gets the end time.
        /// </summary>
        public DateTime? EndTime { get; private set; }

        /// <summary>
        ///     Gets the elapsed.
        /// </summary>
        public TimeSpan Elapsed
        {
            get
            {
                return (this.EndTime.HasValue ? this.EndTime.Value : DateTime.UtcNow) - this.StartTime;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether is finished.
        /// </summary>
        public bool IsFinished
        {
            get
            {
                return this.ExpectedUsers == this.UsersThusFar + this.QueryFailures;
            }
        }

        /// <summary>
        /// Add <see cref="delta"/> users to the running total of <see cref="UsersThusFar"/>
        /// </summary>
        /// <param name="delta">
        /// The delta.
        /// </param>
        /// <returns>
        /// The <see cref="GithubProgressStats"/>.
        /// </returns>
        public GithubProgressStats UserQueriesFinished(int delta = 1)
        {
            return this.Copy(usersThusFar: this.UsersThusFar + delta);
        }

        /// <summary>
        /// Set the <see cref="ExpectedUsers"/> total
        /// </summary>
        /// <param name="totalExpectedUsers">
        /// The total Expected Users.
        /// </param>
        /// <returns>
        /// The <see cref="GithubProgressStats"/>.
        /// </returns>
        public GithubProgressStats SetExpectedUserCount(int totalExpectedUsers)
        {
            return this.Copy(totalExpectedUsers);
        }

        /// <summary>
        /// Add <see cref="delta"/> to the running <see cref="QueryFailures"/> total
        /// </summary>
        /// <param name="delta">
        /// The delta.
        /// </param>
        /// <returns>
        /// The <see cref="GithubProgressStats"/>.
        /// </returns>
        public GithubProgressStats IncrementFailures(int delta = 1)
        {
            return this.Copy(queryFailures: this.QueryFailures + delta);
        }

        /// <summary>
        ///     Query is finished! Set's the <see cref="EndTime" />
        /// </summary>
        /// <returns>
        ///     The <see cref="GithubProgressStats" />.
        /// </returns>
        public GithubProgressStats Finish()
        {
            return this.Copy(endTime: DateTime.UtcNow);
        }

        /// <summary>
        /// Creates a deep copy of the <see cref="GithubProgressStats"/> class
        /// </summary>
        /// <param name="expectedUsers">
        /// The expected Users.
        /// </param>
        /// <param name="usersThusFar">
        /// The users Thus Far.
        /// </param>
        /// <param name="queryFailures">
        /// The query Failures.
        /// </param>
        /// <param name="startTime">
        /// The start Time.
        /// </param>
        /// <param name="endTime">
        /// The end Time.
        /// </param>
        /// <returns>
        /// The <see cref="GithubProgressStats"/>.
        /// </returns>
        public GithubProgressStats Copy(
            int? expectedUsers = null, 
            int? usersThusFar = null, 
            int? queryFailures = null, 
            DateTime? startTime = null, 
            DateTime? endTime = null)
        {
            return new GithubProgressStats(
                startTime ?? this.StartTime, 
                expectedUsers ?? this.ExpectedUsers, 
                usersThusFar ?? this.UsersThusFar, 
                queryFailures ?? this.QueryFailures, 
                endTime ?? this.EndTime);
        }
    }
}