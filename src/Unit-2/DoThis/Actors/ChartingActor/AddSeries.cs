// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddSeries.cs" company="">
//   
// </copyright>
// <summary>
//   Add a new <see cref="Series" /> to the chart
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AddSeries.cs" company="Glass Lewis">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace ChartApp.Actors.ChartingActor
{
    #region Usings

    using System.Windows.Forms.DataVisualization.Charting;

    #endregion

    /// <summary>
    ///     Add a new <see cref="Series" /> to the chart
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
        ///     Gets the series.
        /// </summary>
        public Series Series { get; private set; }
    }
}