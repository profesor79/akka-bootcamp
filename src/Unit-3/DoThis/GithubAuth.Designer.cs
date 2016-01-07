// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GithubAuth.Designer.cs" company="">
//   
// </copyright>
// <summary>
//   The github auth.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GithubActors
{
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    /// The github auth.
    /// </summary>
    partial class GithubAuth
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbOAuth = new System.Windows.Forms.TextBox();
            this.lblAuthStatus = new System.Windows.Forms.Label();
            this.linkGhLabel = new System.Windows.Forms.LinkLabel();
            this.btnAuthenticate = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // label1
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 
                11.25F, 
                System.Drawing.FontStyle.Bold, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)(0));
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "GitHub Access Token";

            // tbOAuth
            this.tbOAuth.Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 
                11.25F, 
                System.Drawing.FontStyle.Bold, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)(0));
            this.tbOAuth.Location = new System.Drawing.Point(253, 7);
            this.tbOAuth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbOAuth.Name = "tbOAuth";
            this.tbOAuth.Size = new System.Drawing.Size(504, 29);
            this.tbOAuth.TabIndex = 1;
            this.tbOAuth.Text = "ac76bea76fbdf9bcc4495db0a140c1b69117af79";

            // lblAuthStatus
            this.lblAuthStatus.AutoSize = true;
            this.lblAuthStatus.Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 
                11.25F, 
                System.Drawing.FontStyle.Regular, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)(0));
            this.lblAuthStatus.Location = new System.Drawing.Point(249, 41);
            this.lblAuthStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAuthStatus.Name = "lblAuthStatus";
            this.lblAuthStatus.Size = new System.Drawing.Size(107, 24);
            this.lblAuthStatus.TabIndex = 2;
            this.lblAuthStatus.Text = "lblGHStatus";
            this.lblAuthStatus.Visible = false;

            // linkGhLabel
            this.linkGhLabel.AutoSize = true;
            this.linkGhLabel.Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 
                11.25F, 
                System.Drawing.FontStyle.Bold, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)(0));
            this.linkGhLabel.Location = new System.Drawing.Point(197, 158);
            this.linkGhLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkGhLabel.Name = "linkGhLabel";
            this.linkGhLabel.Size = new System.Drawing.Size(336, 24);
            this.linkGhLabel.TabIndex = 3;
            this.linkGhLabel.TabStop = true;
            this.linkGhLabel.Text = "How to get a GitHub Access Token";
            this.linkGhLabel.LinkClicked +=
                new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkGhLabelLinkClicked);

            // btnAuthenticate
            this.btnAuthenticate.Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 
                11.25F, 
                System.Drawing.FontStyle.Bold, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)(0));
            this.btnAuthenticate.Location = new System.Drawing.Point(285, 100);
            this.btnAuthenticate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAuthenticate.Name = "btnAuthenticate";
            this.btnAuthenticate.Size = new System.Drawing.Size(181, 39);
            this.btnAuthenticate.TabIndex = 4;
            this.btnAuthenticate.Text = "Authenticate";
            this.btnAuthenticate.UseVisualStyleBackColor = true;
            this.btnAuthenticate.Click += new System.EventHandler(this.BtnAuthenticateClick);

            // GithubAuth
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 191);
            this.Controls.Add(this.btnAuthenticate);
            this.Controls.Add(this.linkGhLabel);
            this.Controls.Add(this.lblAuthStatus);
            this.Controls.Add(this.tbOAuth);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "GithubAuth";
            this.Text = "Sign in to GitHub";
            this.Load += new System.EventHandler(this.GithubAuthLoad);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        /// <summary>
        /// The label 1.
        /// </summary>
        private Label label1;

        /// <summary>
        /// The tb o auth.
        /// </summary>
        private TextBox tbOAuth;

        /// <summary>
        /// The lbl auth status.
        /// </summary>
        private Label lblAuthStatus;

        /// <summary>
        /// The link gh label.
        /// </summary>
        private LinkLabel linkGhLabel;

        /// <summary>
        /// The btn authenticate.
        /// </summary>
        private Button btnAuthenticate;
    }
}