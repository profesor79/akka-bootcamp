// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscribeCounter.cs" company="">
//   
// </copyright>
// <summary>
//   Enables a counter and begins publishing values to <see cref="Subscriber" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SubscribeCounter.cs" company="Glass Lewis">
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
    ///     Enables a counter and begins publishing values to <see cref="Subscriber" />.
    /// </summary>
    public class SubscribeCounter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeCounter"/> class.
        /// </summary>
        /// <param name="counter">
        /// The counter.
        /// </param>
        /// <param name="subscriber">
        /// The subscriber.
        /// </param>
        public SubscribeCounter(CounterType counter, IActorRef subscriber)
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