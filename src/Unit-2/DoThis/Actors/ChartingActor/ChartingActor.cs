// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChartingActor.cs" company="">
//   
// </copyright>
// <summary>
//   The charting actor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ChartingActor.cs" company="Glass Lewis">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace ChartApp.Actors.ChartingActor
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Forms.DataVisualization.Charting;

    using Akka.Actor;

    using ChartApp.Messages;
    using ChartApp.Reporting;

    #endregion

    /// <summary>
    ///     The charting actor.
    /// </summary>
    public class ChartingActor : ReceiveActor
    {
        /// <summary>
        ///     Maximum number of points we will allow in a series
        /// </summary>
        public const int MaxPoints = 250;

        /// <summary>
        ///     The _chart.
        /// </summary>
        private  Chart chart;

        /// <summary>
        ///     The pause button.
        /// </summary>
        private readonly Button pauseButton;

        /// <summary>
        ///     The _series index.
        /// </summary>
        private Dictionary<string, Series> seriesIndex;

        /// <summary>
        ///     Incrementing counter we use to plot along the X-axis
        /// </summary>
        private int xPosCounter;

 
 
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartingActor"/> class.
        /// </summary>
        /// <param name="chart">
        /// The chart.
        /// </param>
        /// <param name="pauseButton">
        /// The pause button.
        /// </param>
        public ChartingActor(Chart chart, Button pauseButton)
            : this(chart, new Dictionary<string, Series>(), pauseButton)
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
        /// <param name="pauseButton">
        /// The pause button.
        /// </param>
        public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex, Button pauseButton)
        {
            this.chart = chart;
            this.seriesIndex = seriesIndex;
            this.pauseButton = pauseButton;
            this.Charting();
        }

        /// <summary>
        ///     The set chart boundaries.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", 
            Justification = "Reviewed. Suppression is OK here.")]
        private void SetChartBoundaries()
        {
            var minAxisY = 0.0d;
            var allPoints = this.seriesIndex.Values.SelectMany(series => series.Points).ToList();
            var yValues = allPoints.SelectMany(point => point.YValues).ToList();
            double maxAxisX = this.xPosCounter;
            double minAxisX = this.xPosCounter - MaxPoints;
            var maxAxisY = yValues.Count > 0 ? Math.Ceiling(yValues.Max()) : 1.0d;
            minAxisY = yValues.Count > 0 ? Math.Floor(yValues.Min()) : 0.0d;
            if (allPoints.Count <= 2)
            {
                return;
            }

            var area = this.chart.ChartAreas[0];
            area.AxisX.Minimum = minAxisX;
            area.AxisX.Maximum = maxAxisX;
            area.AxisY.Minimum = minAxisY;
            area.AxisY.Maximum = maxAxisY;
        }

        /// <summary>
        ///     The charting.
        /// </summary>
        private void Charting()
        {
            this.Receive<InitializeChart>(ic => this.HandleInitialize(ic));
            this.Receive<AddSeries>(addSeries => this.HandleAddSeries(addSeries));
            this.Receive<RemoveSeries>(removeSeries => this.HandleRemoveSeries(removeSeries));
            this.Receive<Metric>(metric => this.HandleMetrics(metric));

            // new receive handler for the TogglePause message type
            this.Receive<TogglePause>(
                pause =>
                    {
                        this.SetPauseButtonText(true);
                        this.BecomeStacked(this.Paused);
                    });
        }

        /// <summary>
        /// The handle metrics paused.
        /// </summary>
        /// <param name="metric">
        /// The metric.
        /// </param>
        private void HandleMetricsPaused(Metric metric)
        {
            if (!string.IsNullOrEmpty(metric.Series) && this.seriesIndex.ContainsKey(metric.Series))
            {
                var series = this.seriesIndex[metric.Series];

                // set the Y value to zero when we're paused
                series.Points.AddXY(this.xPosCounter++, 0.0d);
                while (series.Points.Count > MaxPoints)
                {
                    series.Points.RemoveAt(0);
                }

                this.SetChartBoundaries();
            }
        }

        /// <summary>
        /// The set pause button text.
        /// </summary>
        /// <param name="paused">
        /// The paused.
        /// </param>
        private void SetPauseButtonText(bool paused)
        {
            this.pauseButton.Text = string.Format("{0}", !paused ? "PAUSE ||" : "RESUME ->");
        }

        /// <summary>
        ///     The paused.
        /// </summary>
        private void Paused()
        {
            this.Receive<Metric>(metric => this.HandleMetricsPaused(metric));
            this.Receive<TogglePause>(
                pause =>
                    {
                        this.SetPauseButtonText(false);
                        this.UnbecomeStacked();
                    });
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

            // set the axes up
            var area = this.chart.ChartAreas[0];
            area.AxisX.IntervalType = DateTimeIntervalType.Number;
            area.AxisY.IntervalType = DateTimeIntervalType.Number;

            this.SetChartBoundaries();

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

            this.SetChartBoundaries();
        }

        /// <summary>
        /// The handle add series.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        private void HandleAddSeries(AddSeries series)
        {
            if (!string.IsNullOrEmpty(series.Series.Name) && !this.seriesIndex.ContainsKey(series.Series.Name))
            {
                this.seriesIndex.Add(series.Series.Name, series.Series);
                this.chart.Series.Add(series.Series);
                this.SetChartBoundaries();
            }
        }

        /// <summary>
        /// The handle remove series.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        private void HandleRemoveSeries(RemoveSeries series)
        {
            if (!string.IsNullOrEmpty(series.SeriesName) && this.seriesIndex.ContainsKey(series.SeriesName))
            {
                var seriesToRemove = this.seriesIndex[series.SeriesName];
                this.seriesIndex.Remove(series.SeriesName);
                this.chart.Series.Remove(seriesToRemove);
                this.SetChartBoundaries();
            }
        }

        /// <summary>
        /// The handle metrics.
        /// </summary>
        /// <param name="metric">
        /// The metric.
        /// </param>
        private void HandleMetrics(Metric metric)
        {
            if (!string.IsNullOrEmpty(metric.Series) && this.seriesIndex.ContainsKey(metric.Series))
            {
                var series = this.seriesIndex[metric.Series];
                series.Points.AddXY(this.xPosCounter++, metric.CounterValue);
                while (series.Points.Count > MaxPoints)
                {
                    series.Points.RemoveAt(0);
                }

                this.SetChartBoundaries();
            }
        }

        #endregion
    }
}