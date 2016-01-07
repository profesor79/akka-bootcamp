// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LauncherForm.Designer.cs" company="">
//   
// </copyright>
// <summary>
//   The launcher form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GithubActors
{
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    /// The launcher form.
    /// </summary>
    partial class LauncherForm
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
            this.tbRepoUrl = new System.Windows.Forms.TextBox();
            this.lblRepo = new System.Windows.Forms.Label();
            this.lblIsValid = new System.Windows.Forms.Label();
            this.btnLaunch = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // tbRepoUrl
            this.tbRepoUrl.Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 
                11.25F, 
                System.Drawing.FontStyle.Regular, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)(0));
            this.tbRepoUrl.Location = new System.Drawing.Point(127, 16);
            this.tbRepoUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbRepoUrl.Name = "tbRepoUrl";
            this.tbRepoUrl.Size = new System.Drawing.Size(605, 29);
            this.tbRepoUrl.TabIndex = 0;
            this.tbRepoUrl.Text = "https://github.com/petabridge/akka-bootcamp/";

            // lblRepo
            this.lblRepo.AutoSize = true;
            this.lblRepo.Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 
                11.25F, 
                System.Drawing.FontStyle.Bold, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)(0));
            this.lblRepo.Location = new System.Drawing.Point(4, 20);
            this.lblRepo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRepo.Name = "lblRepo";
            this.lblRepo.Size = new System.Drawing.Size(105, 24);
            this.lblRepo.TabIndex = 1;
            this.lblRepo.Text = "Repo URL";

            // lblIsValid
            this.lblIsValid.AutoSize = true;
            this.lblIsValid.Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 
                11.25F, 
                System.Drawing.FontStyle.Regular, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)(0));
            this.lblIsValid.Location = new System.Drawing.Point(127, 54);
            this.lblIsValid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIsValid.Name = "lblIsValid";
            this.lblIsValid.Size = new System.Drawing.Size(60, 24);
            this.lblIsValid.TabIndex = 2;
            this.lblIsValid.Text = "label1";
            this.lblIsValid.Visible = false;

            // btnLaunch
            this.btnLaunch.Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 
                12F, 
                System.Drawing.FontStyle.Bold, 
                System.Drawing.GraphicsUnit.Point, 
                (byte)(0));
            this.btnLaunch.Location = new System.Drawing.Point(291, 111);
            this.btnLaunch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(189, 46);
            this.btnLaunch.TabIndex = 3;
            this.btnLaunch.Text = "GO";
            this.btnLaunch.UseVisualStyleBackColor = true;
            this.btnLaunch.Click += new System.EventHandler(this.BtnLaunchClick);

            // LauncherForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 186);
            this.Controls.Add(this.btnLaunch);
            this.Controls.Add(this.lblIsValid);
            this.Controls.Add(this.lblRepo);
            this.Controls.Add(this.tbRepoUrl);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "LauncherForm";
            this.Text = "Who Starred This Repo?";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LauncherFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        /// <summary>
        /// The tb repo url.
        /// </summary>
        private TextBox tbRepoUrl;

        /// <summary>
        /// The lbl repo.
        /// </summary>
        private Label lblRepo;

        /// <summary>
        /// The lbl is valid.
        /// </summary>
        private Label lblIsValid;

        /// <summary>
        /// The btn launch.
        /// </summary>
        private Button btnLaunch;
    }
}

