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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.sysChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.AddChartSeries = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sysChart)).BeginInit();
            this.SuspendLayout();
            // 
            // sysChart
            // 
            chartArea2.Name = "ChartArea1";
            this.sysChart.ChartAreas.Add(chartArea2);
            this.sysChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.sysChart.Legends.Add(legend2);
            this.sysChart.Location = new System.Drawing.Point(0, 0);
            this.sysChart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sysChart.Name = "sysChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.sysChart.Series.Add(series2);
            this.sysChart.Size = new System.Drawing.Size(912, 549);
            this.sysChart.TabIndex = 0;
            this.sysChart.Text = "sysChart";
            // 
            // AddChartSeries
            // 
            this.AddChartSeries.Location = new System.Drawing.Point(760, 462);
            this.AddChartSeries.Name = "AddChartSeries";
            this.AddChartSeries.Size = new System.Drawing.Size(124, 45);
            this.AddChartSeries.TabIndex = 1;
            this.AddChartSeries.Text = "Add New Series";
            this.AddChartSeries.UseVisualStyleBackColor = true;
            this.AddChartSeries.Click += new System.EventHandler(this.AddChartSeriesClick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 549);
            this.Controls.Add(this.AddChartSeries);
            this.Controls.Add(this.sysChart);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.Button AddChartSeries;
    }
}

