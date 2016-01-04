// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Unwatch.cs" company="">
//   
// </copyright>
// <summary>
//   Unsubscribe the <see cref="ChartApp.Actors.ChartingActor" /> to updates for <see cref="Counter" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ChartApp.Actors.PerformanceCounterCoordinatorActor
{
    using ChartApp.Enums;

    /// <summary>
    /// Unsubscribe the <see cref="ChartApp.Actors.ChartingActor"/> to updates for <see cref="Counter"/>
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
        /// Gets the counter.
        /// </summary>
        public CounterType Counter { get; private set; }
    }
}