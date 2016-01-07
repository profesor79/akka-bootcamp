#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Program.cs" company="none">
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

    #endregion

    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     ActorSystem we'llbe using to collect and process data
        ///     from Github using their official .NET SDK, Octokit
        /// </summary>
        public static ActorSystem GithubActors;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            GithubActors = ActorSystem.Create("GithubActors");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GithubAuth());
        }
    }
}