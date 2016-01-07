// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActorPaths.cs" company="">
//   
// </copyright>
// <summary>
//   Static helper class used to define paths to fixed-name actors
//   (helps eliminate errors when using )
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ActorPaths.cs" company="none">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace GithubActors
{
    #region Usings

    using Akka.Actor;

    #endregion

    /// <summary>
    ///     Static helper class used to define paths to fixed-name actors
    ///     (helps eliminate errors when using <see cref="ActorSelection" />)
    /// </summary>
    public static class ActorPaths
    {
        /// <summary>
        ///     The github authenticator actor.
        /// </summary>
        public static readonly ActorMetaData GithubAuthenticatorActor = new ActorMetaData(
            "authenticator", 
            "akka://GithubActors/user/authenticator");

        /// <summary>
        ///     The main form actor.
        /// </summary>
        public static readonly ActorMetaData MainFormActor = new ActorMetaData(
            "mainform", 
            "akka://GithubActors/user/mainform");

        /// <summary>
        ///     The github validator actor.
        /// </summary>
        public static readonly ActorMetaData GithubValidatorActor = new ActorMetaData(
            "validator", 
            "akka://GithubActors/user/validator");

        /// <summary>
        ///     The github commander actor.
        /// </summary>
        public static readonly ActorMetaData GithubCommanderActor = new ActorMetaData(
            "commander", 
            "akka://GithubActors/user/commander");

        /// <summary>
        ///     The github coordinator actor.
        /// </summary>
        public static readonly ActorMetaData GithubCoordinatorActor = new ActorMetaData(
            "coordinator", 
            "akka://GithubActors/user/commander/coordinator");
    }

    /// <summary>
    ///     Meta-data class
    /// </summary>
    public class ActorMetaData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActorMetaData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        public ActorMetaData(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Gets the path.
        /// </summary>
        public string Path { get; private set; }
    }
}