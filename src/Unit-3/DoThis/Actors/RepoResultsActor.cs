// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepoResultsActor.cs" company="">
//   
// </copyright>
// <summary>
//   Actor responsible for printing the results and progress from a
//   onto a  (runs on the UI thread)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="RepoResultsActor.cs" company="none">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace GithubActors.Actors
{
    #region Usings

    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using Akka.Actor;

    #endregion

    /// <summary>
    ///     Actor responsible for printing the results and progress from a <see cref="GithubCoordinatorActor" />
    ///     onto a <see cref="RepoResultsForm" /> (runs on the UI thread)
    /// </summary>
    public class RepoResultsActor : ReceiveActor
    {
        /// <summary>
        ///     The _has set progress.
        /// </summary>
        private readonly bool hasSetProgress = false;

        /// <summary>
        ///     The _progress bar.
        /// </summary>
        private readonly ToolStripProgressBar progressBar;

        /// <summary>
        ///     The _status label.
        /// </summary>
        private readonly ToolStripStatusLabel statusLabel;

        /// <summary>
        ///     The _user dg.
        /// </summary>
        private readonly DataGridView userDg;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepoResultsActor"/> class.
        /// </summary>
        /// <param name="userDg">
        /// The user dg.
        /// </param>
        /// <param name="statusLabel">
        /// The status label.
        /// </param>
        /// <param name="progressBar">
        /// The progress bar.
        /// </param>
        public RepoResultsActor(DataGridView userDg, ToolStripStatusLabel statusLabel, ToolStripProgressBar progressBar)
        {
            this.userDg = userDg;
            this.statusLabel = statusLabel;
            this.progressBar = progressBar;
            this.InitialReceives();
        }

        /// <summary>
        ///     The initial receives.
        /// </summary>
        private void InitialReceives()
        {
            // progress update
            this.Receive<GithubProgressStats>(
                stats =>
                    {
                        // time to start animating the progress bar
                        if (!this.hasSetProgress && stats.ExpectedUsers > 0)
                        {
                            this.progressBar.Minimum = 0;
                            this.progressBar.Step = 1;
                            this.progressBar.Maximum = stats.ExpectedUsers;
                            this.progressBar.Value = stats.UsersThusFar;
                            this.progressBar.Visible = true;
                            this.statusLabel.Visible = true;
                        }

                        this.statusLabel.Text = string.Format(
                            "{0} out of {1} users ({2} failures) [{3} elapsed]", 
                            stats.UsersThusFar, 
                            stats.ExpectedUsers, 
                            stats.QueryFailures, 
                            stats.Elapsed);
                        this.progressBar.Value = stats.UsersThusFar + stats.QueryFailures;
                    });

            // user update
            this.Receive<IEnumerable<SimilarRepo>>(
                repos =>
                    {
                        foreach (var similarRepo in repos)
                        {
                            var repo = similarRepo.Repo;
                            var row = new DataGridViewRow();
                            row.CreateCells(this.userDg);
                            row.Cells[0].Value = repo.Owner.Login;
                            row.Cells[1].Value = repo.Name;
                            row.Cells[2].Value = repo.HtmlUrl;
                            row.Cells[3].Value = similarRepo.SharedStarrers;
                            row.Cells[4].Value = repo.SubscribersCount;
                            row.Cells[5].Value = repo.StargazersCount;
                            row.Cells[6].Value = repo.ForksCount;
                            this.userDg.Rows.Add(row);
                        }
                    });

            // critical failure, like not being able to connect to Github
            this.Receive<GithubCoordinatorActor.JobFailed>(
                failed =>
                    {
                        this.progressBar.Visible = true;
                        this.progressBar.ForeColor = Color.Red;
                        this.progressBar.Maximum = 1;
                        this.progressBar.Value = 1;
                        this.statusLabel.Visible = true;
                        this.statusLabel.Text = string.Format(
                            "Failed to gather data for Github repository {0} / {1}", 
                            failed.Repo.Owner, 
                            failed.Repo.Repo);
                    });
        }
    }
}