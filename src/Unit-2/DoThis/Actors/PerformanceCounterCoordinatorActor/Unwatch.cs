// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Unwatch.cs" company="">
//   
// </copyright>
// <summary>
//   Unsubscribe the <see cref="ChartApp.Actors.ChartingActor" /> to updates for <see cref="Counter" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Unwatch.cs" company="Glass Lewis">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace ChartApp.Actors.PerformanceCounterCoordinatorActor
{
    #region Usings

    using ChartApp.Enums;

    #endregion

    /// <summary>
    ///     Unsubscribe the <see cref="ChartApp.Actors.ChartingActor" /> to updates for <see cref="Counter" />
    /// </summary>
    public class Unwatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Unwatch"/> class.
        /// </summary>
        /// <param name="counter">
        /// The counter.
        /// </param>
        public Unwatch(CounterType counter)
        {
            this.Counter = counter;
        }

        /// <summary>
        ///     Gets the counter.
        /// </summary>
        public CounterType Counter { get; private set; }
    }
}