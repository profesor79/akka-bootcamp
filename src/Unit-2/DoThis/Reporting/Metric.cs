// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Metric.cs" company="">
//   
// </copyright>
// <summary>
//   Metric data at the time of sample
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Metric.cs" company="Glass Lewis">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace ChartApp.Reporting
{
    /// <summary>
    ///     Metric data at the time of sample
    /// </summary>
    public class Metric
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Metric"/> class.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        /// <param name="counterValue">
        /// The counter value.
        /// </param>
        public Metric(string series, float counterValue)
        {
            this.CounterValue = counterValue;
            this.Series = series;
        }

        /// <summary>
        ///     Gets the series.
        /// </summary>
        public string Series { get; private set; }

        /// <summary>
        ///     Gets the counter value.
        /// </summary>
        public float CounterValue { get; private set; }
    }
}