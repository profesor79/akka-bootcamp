// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChartDataHelper.cs" company="">
//   
// </copyright>
// <summary>
//   Helper class for creating random data for chart plots
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ChartDataHelper.cs" company="Glass Lewis">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace ChartApp
{
    #region Usings

    using System;
    using System.Linq;
    using System.Windows.Forms.DataVisualization.Charting;

    using Akka.Util;

    #endregion

    /// <summary>
    ///     Helper class for creating random data for chart plots
    /// </summary>
    public static class ChartDataHelper
    {
        /// <summary>
        /// The random series.
        /// </summary>
        /// <param name="seriesName">
        /// The series name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <returns>
        /// The <see cref="Series"/>.
        /// </returns>
        public static Series RandomSeries(
            string seriesName, 
            SeriesChartType type = SeriesChartType.Line, 
            int points = 100)
        {
            var series = new Series(seriesName) { ChartType = type };
            foreach (var i in Enumerable.Range(0, points))
            {
                var rng = ThreadLocalRandom.Current.NextDouble();
                series.Points.Add(new DataPoint(i, 2.0 * Math.Sin(rng) + Math.Sin(rng / 4.5)));
            }

            series.BorderWidth = 3;
            return series;
        }
    }
}