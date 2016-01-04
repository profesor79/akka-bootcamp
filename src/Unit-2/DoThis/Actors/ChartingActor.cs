// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChartingActor.cs" company="">
//   
// </copyright>
// <summary>
//   The charting actor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------




namespace ChartApp.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms.DataVisualization.Charting;

    using Akka.Actor;

    /// <summary>
    /// The charting actor.
    /// </summary>
    public class ChartingActor : ReceiveActor
    {
        /// <summary>
        /// The _chart.
        /// </summary>
        private readonly Chart chart;

        /// <summary>
        /// The _series index.
        /// </summary>
        private Dictionary<string, Series> seriesIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartingActor"/> class.
        /// </summary>
        /// <param name="chart">
        /// The chart.
        /// </param>
        public ChartingActor(Chart chart) : this(chart, new Dictionary<string, Series>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartingActor"/> class.
        /// </summary>
        /// <param name="chart">
        /// The chart.
        /// </param>
        /// <param name="seriesIndex">
        /// The series index.
        /// </param>
        public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex)
        {
            this.chart = chart;
            this.seriesIndex = seriesIndex;

            this.Receive<InitializeChart>(ic => this.HandleInitialize(ic));
            this.Receive<AddSeries>(addSeries => this.HandleAddSeries(addSeries));
        }

        /// <summary>
        /// The handle add series.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        private void HandleAddSeries(AddSeries series)
        {
            if (!string.IsNullOrEmpty(series.Series.Name) &&
            !this.seriesIndex.ContainsKey(series.Series.Name))
            {
                this.seriesIndex.Add(series.Series.Name, series.Series);
                this.chart.Series.Add(series.Series);
            }
        }

        #region Individual Message Type Handlers

        /// <summary>
        /// The handle initialize.
        /// </summary>
        /// <param name="ic">
        /// The ic.
        /// </param>
        private void HandleInitialize(InitializeChart ic)
        {
            if (ic.InitialSeries != null)
            {
                // swap the two series out
                this.seriesIndex = ic.InitialSeries;
            }

            // delete any existing series
            this.chart.Series.Clear();

            // attempt to render the initial chart
            if (this.seriesIndex.Any())
            {
                foreach (var series in this.seriesIndex)
                {
                    // force both the chart and the internal index to use the same names
                    series.Value.Name = series.Key;
                    this.chart.Series.Add(series.Value);
                }
            }
        }

        #endregion
    }
}
