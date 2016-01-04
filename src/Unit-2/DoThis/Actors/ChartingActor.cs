// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChartingActor.cs" company="">
//   
// </copyright>
// <summary>
//   The charting actor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

using Akka.Actor;

namespace ChartApp.Actors
{
    /// <summary>
    /// The charting actor.
    /// </summary>
    public class ChartingActor : UntypedActor
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
        }

        /// <summary>
        /// The on receive.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void OnReceive(object message)
        {
            if (message is InitializeChart)
            {
                var ic = message as InitializeChart;
                this.HandleInitialize(ic);
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
