// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GithubValidatorActor.cs" company="">
//   
// </copyright>
// <summary>
//   Actor has one job - ensure that a public repo exists at the specified address
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GithubValidatorActor.cs" company="none">
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
    using System.Linq;

    using Akka.Actor;

    using Octokit;

    #endregion

    /// <summary>
    ///     Actor has one job - ensure that a public repo exists at the specified address
    /// </summary>
    public class GithubValidatorActor : ReceiveActor
    {
        /// <summary>
        ///     The _git hub client.
        /// </summary>
        private readonly IGitHubClient gitHubClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubValidatorActor"/> class.
        /// </summary>
        /// <param name="gitHubClient">
        /// The git hub client.
        /// </param>
        public GithubValidatorActor(IGitHubClient gitHubClient)
        {
            this.gitHubClient = gitHubClient;
            this.ReadyToValidate();
        }

        /// <summary>
        ///     The ready to validate.
        /// </summary>
        private void ReadyToValidate()
        {
            // Outright invalid URLs
            this.Receive<ValidateRepo>(
                repo => string.IsNullOrEmpty(repo.RepoUri) || !Uri.IsWellFormedUriString(repo.RepoUri, UriKind.Absolute), 
                repo => this.Sender.Tell(new InvalidRepo(repo.RepoUri, "Not a valid absolute URI")));

            // Repos that at least have a valid absolute URL
            this.Receive<ValidateRepo>(
                repo =>
                    {
                        var userOwner = SplitIntoOwnerAndRepo(repo.RepoUri);

                        // close over the sender in an instance variable
                        var sender = this.Sender;
                        this.gitHubClient.Repository.Get(userOwner.Item1, userOwner.Item2).ContinueWith<object>(
                            t =>
                                {
                                    // Rule #1 of async in Akka.NET - turn exceptions into messages your actor understands
                                    if (t.IsCanceled)
                                    {
                                        return new InvalidRepo(repo.RepoUri, "Repo lookup timed out");
                                    }

                                    if (t.IsFaulted)
                                    {
                                        return new InvalidRepo(
                                            repo.RepoUri, 
                                            t.Exception != null
                                                ? t.Exception.GetBaseException().Message
                                                : "Unknown Octokit error");
                                    }

                                    return t.Result;
                                }).PipeTo(this.Self, sender);
                    });

            // something went wrong while querying github, sent to ourselves via PipeTo
            // however - Sender gets preserved on the call, so it's safe to use Forward here.
            this.Receive<InvalidRepo>(repo => this.Sender.Forward(repo));

            // Octokit was able to retrieve this repository
            this.Receive<Repository>(
                repository =>
                    {
                        // ask the GithubCommander if we can accept this job
                        Context.ActorSelection(ActorPaths.GithubCommanderActor.Path)
                            .Tell(
                                new GithubCommanderActor.CanAcceptJob(
                                    new RepoKey(repository.Owner.Login, repository.Name)));
                    });

            /* REPO is valid, but can we process it at this time? */

            // yes
            this.Receive<GithubCommanderActor.UnableToAcceptJob>(
                job => Context.ActorSelection(ActorPaths.MainFormActor.Path).Tell(job));

            // no
            this.Receive<GithubCommanderActor.AbleToAcceptJob>(
                job => Context.ActorSelection(ActorPaths.MainFormActor.Path).Tell(job));
        }

        /// <summary>
        /// The split into owner and repo.
        /// </summary>
        /// <param name="repoUri">
        /// The repo uri.
        /// </param>
        /// <returns>
        /// The <see cref="Tuple"/>.
        /// </returns>
        public static Tuple<string, string> SplitIntoOwnerAndRepo(string repoUri)
        {
            var split = new Uri(repoUri, UriKind.Absolute).PathAndQuery.TrimEnd('/').Split('/').Reverse().ToList();

            // uri path without trailing slash
            return Tuple.Create(split[1], split[0]); // User, Repo
        }

        #region Messages

        /// <summary>
        ///     The validate repo.
        /// </summary>
        public class ValidateRepo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ValidateRepo"/> class.
            /// </summary>
            /// <param name="repoUri">
            /// The repo uri.
            /// </param>
            public ValidateRepo(string repoUri)
            {
                this.RepoUri = repoUri;
            }

            /// <summary>
            ///     Gets the repo uri.
            /// </summary>
            public string RepoUri { get; private set; }
        }

        /// <summary>
        ///     The invalid repo.
        /// </summary>
        public class InvalidRepo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InvalidRepo"/> class.
            /// </summary>
            /// <param name="repoUri">
            /// The repo uri.
            /// </param>
            /// <param name="reason">
            /// The reason.
            /// </param>
            public InvalidRepo(string repoUri, string reason)
            {
                this.Reason = reason;
                this.RepoUri = repoUri;
            }

            /// <summary>
            ///     Gets the repo uri.
            /// </summary>
            public string RepoUri { get; private set; }

            /// <summary>
            ///     Gets the reason.
            /// </summary>
            public string Reason { get; private set; }
        }

        /// <summary>
        ///     System is unable to process additional repos at this time
        /// </summary>
        public class SystemBusy
        {
        }

        /// <summary>
        ///     This is a valid repository
        /// </summary>
        public class RepoIsValid
        {
            /// <summary>
            ///     The _instance.
            /// </summary>
            private static readonly RepoIsValid _instance = new RepoIsValid();

            /*
             * Using singleton pattern here since it's a stateless message.
             * 
             * Considered to be a good practice to eliminate unnecessary garbage collection,
             * and it's used internally inside Akka.NET for similar scenarios.
             */

            /// <summary>
            ///     Prevents a default instance of the <see cref="RepoIsValid" /> class from being created.
            /// </summary>
            private RepoIsValid()
            {
            }

            /// <summary>
            ///     Gets the instance.
            /// </summary>
            public static RepoIsValid Instance
            {
                get
                {
                    return _instance;
                }
            }
        }

        #endregion
    }
}