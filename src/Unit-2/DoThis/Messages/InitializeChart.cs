// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitializeChart.cs" company="">
//   
// </copyright>
// <summary>
//   The initialize chart.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ChartApp.Messages
{
    using System.Collections.Generic;
    using System.Windows.Forms.DataVisualization.Charting;

    /// <summary>
    /// The initialize chart.
    /// </summary>
    public class InitializeChart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeChart"/> class.
        /// </summary>
        /// <param name="initialSeries">
        /// The initial series.
        /// </param>
        public InitializeChart(Dictionary<string, Series> initialSeries)
        {
            this.InitialSeries = initialSeries;
        }

        /// <summary>
        /// Gets the initial series.
        /// </summary>
        public Dictionary<string, Series> InitialSeries { get; private set; }
    }
}