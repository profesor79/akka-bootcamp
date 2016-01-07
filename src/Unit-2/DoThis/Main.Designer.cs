// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.Designer.cs" company="">
//   
// </copyright>
// <summary>
//   The main.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ChartApp
{
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Windows.Forms.DataVisualization.Charting;

    /// <summary>
    /// The main.
    /// </summary>
    partial class Main
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.sysChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.CpuToggle = new System.Windows.Forms.Button();
            this.MemoryToggle = new System.Windows.Forms.Button();
            this.DiskToggle = new System.Windows.Forms.Button();
            this.PauseToggle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sysChart)).BeginInit();
            this.SuspendLayout();
            // 
            // sysChart
            // 
            chartArea1.Name = "ChartArea1";
            this.sysChart.ChartAreas.Add(chartArea1);
            this.sysChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.sysChart.Legends.Add(legend1);
            this.sysChart.Location = new System.Drawing.Point(0, 0);
            this.sysChart.Margin = new System.Windows.Forms.Padding(4);
            this.sysChart.Name = "sysChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.sysChart.Series.Add(series1);
            this.sysChart.Size = new System.Drawing.Size(912, 549);
            this.sysChart.TabIndex = 0;
            this.sysChart.Text = "sysChart";
            // 
            // CpuToggle
            // 
            this.CpuToggle.Location = new System.Drawing.Point(762, 367);
            this.CpuToggle.Name = "CpuToggle";
            this.CpuToggle.Size = new System.Drawing.Size(138, 39);
            this.CpuToggle.TabIndex = 1;
            this.CpuToggle.Text = "CPU (ON)";
            this.CpuToggle.UseVisualStyleBackColor = true;
            this.CpuToggle.Click += new System.EventHandler(this.CpuToggleClick);
            // 
            // MemoryToggle
            // 
            this.MemoryToggle.Location = new System.Drawing.Point(762, 411);
            this.MemoryToggle.Name = "MemoryToggle";
            this.MemoryToggle.Size = new System.Drawing.Size(138, 39);
            this.MemoryToggle.TabIndex = 1;
            this.MemoryToggle.Text = "MEMEORY (OFF)";
            this.MemoryToggle.UseVisualStyleBackColor = true;
            this.MemoryToggle.Click += new System.EventHandler(this.MemoryToggleClick);
            // 
            // DiskToggle
            // 
            this.DiskToggle.Location = new System.Drawing.Point(762, 455);
            this.DiskToggle.Name = "DiskToggle";
            this.DiskToggle.Size = new System.Drawing.Size(138, 39);
            this.DiskToggle.TabIndex = 1;
            this.DiskToggle.Text = "DISK (OFF)";
            this.DiskToggle.UseVisualStyleBackColor = true;
            this.DiskToggle.Click += new System.EventHandler(this.DiskToggleClick);
            // 
            // PauseToggle
            // 
            this.PauseToggle.Location = new System.Drawing.Point(762, 322);
            this.PauseToggle.Name = "PauseToggle";
            this.PauseToggle.Size = new System.Drawing.Size(138, 39);
            this.PauseToggle.TabIndex = 1;
            this.PauseToggle.Text = "PAUSE";
            this.PauseToggle.UseVisualStyleBackColor = true;
            this.PauseToggle.Click += new System.EventHandler(this.PauseToggleClick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 549);
            this.Controls.Add(this.DiskToggle);
            this.Controls.Add(this.MemoryToggle);
            this.Controls.Add(this.PauseToggle);
            this.Controls.Add(this.CpuToggle);
            this.Controls.Add(this.sysChart);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "System Metrics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormClosing);
            this.Load += new System.EventHandler(this.MainLoad);
            ((System.ComponentModel.ISupportInitialize)(this.sysChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// The sys chart.
        /// </summary>
        private Chart sysChart;

        /// <summary>
        /// The cpu toggle.
        /// </summary>
        private Button CpuToggle;

        /// <summary>
        /// The memory toggle.
        /// </summary>
        private Button MemoryToggle;

        /// <summary>
        /// The disk toggle.
        /// </summary>
        private Button DiskToggle;
        private Button PauseToggle;
    }
}

