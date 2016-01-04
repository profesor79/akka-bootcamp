// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Watch.cs" company="">
//   
// </copyright>
// <summary>
//   Subscribe the <see cref="ChartApp.Actors.ChartingActor" /> to updates for <see cref="Counter" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ChartApp.Actors.PerformanceCounterCoordinatorActor
{
    using ChartApp.Enums;

    /// <summary>
    /// Subscribe the <see cref="ChartApp.Actors.ChartingActor"/> to updates for <see cref="Counter"/>.
    /// </summary>
    public class Watch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Watch"/> class.
        /// </summary>
        /// <param name="counter">
        /// The counter.
        /// </param>
        public Watch(CounterType counter)
        {
            this.Counter = counter;
        }

        /// <summary>
        /// Gets the counter.
        /// </summary>
        public CounterType Counter { get; private set; }
    }
}