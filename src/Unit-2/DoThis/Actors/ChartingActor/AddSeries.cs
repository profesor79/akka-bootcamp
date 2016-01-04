// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddSeries.cs" company="">
//   
// </copyright>
// <summary>
//   Add a new  to the chart
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ChartApp.Actors.ChartingActor
{
    using System.Windows.Forms.DataVisualization.Charting;

    /// <summary>
    /// Add a new <see cref="Series"/> to the chart
    /// </summary>
    public class AddSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddSeries"/> class.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        public AddSeries(Series series)
        {
            this.Series = series;
        }

        /// <summary>
        /// Gets the series.
        /// </summary>
        public Series Series { get; private set; }
    }
}
