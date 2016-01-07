// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveSeries.cs" company="">
//   
// </copyright>
// <summary>
//   Remove an existing <see cref="Series" /> from the chart
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="RemoveSeries.cs" company="Glass Lewis">
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
    ///     Remove an existing <see cref="Series" /> from the chart
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
        ///     Gets the series name.
        /// </summary>
        public string SeriesName { get; private set; }
    }
}