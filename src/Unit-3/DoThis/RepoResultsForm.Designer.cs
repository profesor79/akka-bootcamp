// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepoResultsForm.Designer.cs" company="">
//   
// </copyright>
// <summary>
//   The repo results form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GithubActors
{
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    /// The repo results form.
    /// </summary>
    partial class RepoResultsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// true if managed resources should be disposed; otherwise, false.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgUsers = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.Owner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RepoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.URL = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Shared = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Watchers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stars = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Forks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)this.dgUsers).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();

            // dgUsers
            this.dgUsers.AllowUserToOrderColumns = true;
            this.dgUsers.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUsers.Columns.AddRange(
                new DataGridViewColumn[]
                    {
                       this.Owner, this.RepoName, this.URL, this.Shared, this.Watchers, this.Stars, this.Forks 
                    });
            this.dgUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgUsers.Location = new System.Drawing.Point(0, 0);
            this.dgUsers.Name = "dgUsers";
            this.dgUsers.Size = new System.Drawing.Size(739, 322);
            this.dgUsers.TabIndex = 0;

            // statusStrip1
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.tsProgress, this.tsStatus });
            this.statusStrip1.Location = new System.Drawing.Point(0, 300);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(739, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";

            // tsProgress
            this.tsProgress.Name = "tsProgress";
            this.tsProgress.Size = new System.Drawing.Size(100, 16);
            this.tsProgress.Visible = false;

            // tsStatus
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(73, 17);
            this.tsStatus.Text = "Processing...";
            this.tsStatus.Visible = false;

            // Owner
            this.Owner.HeaderText = "Owner";
            this.Owner.Name = "Owner";

            // RepoName
            this.RepoName.HeaderText = "Name";
            this.RepoName.Name = "RepoName";

            // URL
            this.URL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.URL.HeaderText = "URL";
            this.URL.Name = "URL";

            // Shared
            this.Shared.HeaderText = "SharedStars";
            this.Shared.Name = "Shared";

            // Watchers
            this.Watchers.HeaderText = "Watchers";
            this.Watchers.Name = "Watchers";

            // Stars
            this.Stars.HeaderText = "Stars";
            this.Stars.Name = "Stars";

            // Forks
            this.Forks.HeaderText = "Forks";
            this.Forks.Name = "Forks";

            // RepoResultsForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 322);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dgUsers);
            this.Name = "RepoResultsForm";
            this.Text = "Repos Similar to {RepoName}";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RepoResultsFormFormClosing);
            this.Load += new System.EventHandler(this.RepoResultsFormLoad);
            ((System.ComponentModel.ISupportInitialize)this.dgUsers).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        /// <summary>
        /// The dg users.
        /// </summary>
        private DataGridView dgUsers;

        /// <summary>
        /// The status strip 1.
        /// </summary>
        private StatusStrip statusStrip1;

        /// <summary>
        /// The ts progress.
        /// </summary>
        private ToolStripProgressBar tsProgress;

        /// <summary>
        /// The ts status.
        /// </summary>
        private ToolStripStatusLabel tsStatus;

        /// <summary>
        /// The owner.
        /// </summary>
        private DataGridViewTextBoxColumn Owner;

        /// <summary>
        /// The repo name.
        /// </summary>
        private DataGridViewTextBoxColumn RepoName;

        /// <summary>
        /// The url.
        /// </summary>
        private DataGridViewLinkColumn URL;

        /// <summary>
        /// The shared.
        /// </summary>
        private DataGridViewTextBoxColumn Shared;

        /// <summary>
        /// The watchers.
        /// </summary>
        private DataGridViewTextBoxColumn Watchers;

        /// <summary>
        /// The stars.
        /// </summary>
        private DataGridViewTextBoxColumn Stars;

        /// <summary>
        /// The forks.
        /// </summary>
        private DataGridViewTextBoxColumn Forks;
    }
}