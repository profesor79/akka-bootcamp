// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Program.cs" company="Glass Lewis">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace ChartApp
{
    #region Usings

    using System;
    using System.Windows.Forms;

    using Akka.Actor;

    #endregion

    /// <summary>
    ///     The program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     ActorSystem we'll be using to publish data to charts
        ///     and subscribe from performance counters
        /// </summary>
        public static ActorSystem ChartActors;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ChartActors = ActorSystem.Create("ChartActors");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}