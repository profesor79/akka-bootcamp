// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnsubscribeCounter.cs" company="">
//   
// </copyright>
// <summary>
//   Unsubscribes <see cref="Subscriber" /> from receiving updates for a given counter
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="UnsubscribeCounter.cs" company="Glass Lewis">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace ChartApp.Reporting
{
    #region Usings

    using Akka.Actor;

    using ChartApp.Enums;

    #endregion

    /// <summary>
    ///     Unsubscribes <see cref="Subscriber" /> from receiving updates for a given counter
    /// </summary>
    public class UnsubscribeCounter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsubscribeCounter"/> class.
        /// </summary>
        /// <param name="counter">
        /// The counter.
        /// </param>
        /// <param name="subscriber">
        /// The subscriber.
        /// </param>
        public UnsubscribeCounter(CounterType counter, IActorRef subscriber)
        {
            this.Subscriber = subscriber;
            this.Counter = counter;
        }

        /// <summary>
        ///     Gets the counter.
        /// </summary>
        public CounterType Counter { get; private set; }

        /// <summary>
        ///     Gets the subscriber.
        /// </summary>
        public IActorRef Subscriber { get; private set; }
    }
}