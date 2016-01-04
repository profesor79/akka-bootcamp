﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChartingActor.cs" company="">
//   
// </copyright>
// <summary>
//   The charting actor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ChartApp.Actors.ChartingActor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms.DataVisualization.Charting;

    using Akka.Actor;

    using ChartApp.Messages;
    using ChartApp.Reporting;

    /// <summary>
    /// The charting actor.
    /// </summary>
    public class ChartingActor : ReceiveActor
    {
        /// <summary>
        /// Maximum number of points we will allow in a series
        /// </summary>
        public const int MaxPoints = 250;

        /// <summary>
        /// Incrementing counter we use to plot along the X-axis
        /// </summary>
        private int xPosCounter = 0;

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
        public ChartingActor(Chart chart)
            : this(chart, new Dictionary<string, Series>())
        {
        }

        /// <summary>
        /// The set chart boundaries.
        /// </summary>
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
            this.Receive<RemoveSeries>(removeSeries => this.HandleRemoveSeries(removeSeries));
            this.Receive<Metric>(metric => this.HandleMetrics(metric));
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
            if (!string.IsNullOrEmpty(series.Series.Name) &&
                !this.seriesIndex.ContainsKey(series.Series.Name))
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
                while (series.Points.Count > MaxPoints) series.Points.RemoveAt(0);
                this.SetChartBoundaries();
            }
        }

        #endregion
    }
}
