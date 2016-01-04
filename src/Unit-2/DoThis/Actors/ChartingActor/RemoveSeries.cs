// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveSeries.cs" company="">
//   
// </copyright>
// <summary>
//   Remove an existing  from the chart
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ChartApp.Actors.ChartingActor
{
    using System.Windows.Forms.DataVisualization.Charting;

    /// <summary>
    /// Remove an existing <see cref="Series"/> from the chart
    /// </summary>
    public class RemoveSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveSeries"/> class.
        /// </summary>
        /// <param name="seriesName">
        /// The series name.
        /// </param>
        public RemoveSeries(string seriesName)
        {
            this.SeriesName = seriesName;
        }

        /// <summary>
        /// Gets the series name.
        /// </summary>
        public string SeriesName { get; private set; }
    }
}
