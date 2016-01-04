// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceCounterActor.cs" company="">
//   
// </copyright>
// <summary>
//   Actor responsible for monitoring a specific
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ChartApp.Actors.PerformanceCounterActor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Akka.Actor;

    using ChartApp.Reporting;

    /// <summary>
    /// Actor responsible for monitoring a specific <see cref="PerformanceCounter"/>
    /// </summary>
    public class PerformanceCounterActor : UntypedActor
    {
        /// <summary>
        /// The series name.
        /// </summary>
        private readonly string seriesName;

        /// <summary>
        /// The performance counter generator.
        /// </summary>
        private readonly Func<PerformanceCounter> performanceCounterGenerator;

        /// <summary>
        /// The counter.
        /// </summary>
        private PerformanceCounter counter;

        /// <summary>
        /// The subscriptions.
        /// </summary>
        private readonly HashSet<IActorRef> subscriptions;

        /// <summary>
        /// The cancel publishing.
        /// </summary>
        private readonly ICancelable cancelPublishing;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounterActor"/> class.
        /// </summary>
        /// <param name="seriesName">
        /// The series name.
        /// </param>
        /// <param name="performanceCounterGenerator">
        /// The performance counter generator.
        /// </param>
        public PerformanceCounterActor(string seriesName, Func<PerformanceCounter> performanceCounterGenerator)
        {
            this.seriesName = seriesName;
            this.performanceCounterGenerator = performanceCounterGenerator;
            this.subscriptions = new HashSet<IActorRef>();
            this.cancelPublishing = new Cancelable(Context.System.Scheduler);
        }

        #region Actor lifecycle methods

        /// <summary>
        /// The pre start.
        /// </summary>
        protected override void PreStart()
        {
            // create a new instance of the performance counter
            this.counter = this.performanceCounterGenerator();
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromMilliseconds(250), 
                TimeSpan.FromMilliseconds(250), this.Self, 
                new GatherMetrics(), this.Self, this.cancelPublishing);
        }

        /// <summary>
        /// The post stop.
        /// </summary>
        protected override void PostStop()
        {
            try
            {
                // terminate the scheduled task
                this.cancelPublishing.Cancel(false);
                this.counter.Dispose();
            }
            catch
            {
                // don't care about additional "ObjectDisposed" exceptions
            }
            finally
            {
                base.PostStop();
            }
        }

        #endregion

        /// <summary>
        /// The on receive.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void OnReceive(object message)
        {
            if (message is GatherMetrics)
            {
                // publish latest counter value to all subscribers
                var metric = new Metric(this.seriesName, this.counter.NextValue());
                foreach(var sub in this.subscriptions)
                    sub.Tell(metric);
            }
            else if (message is SubscribeCounter)
            {
                // add a subscription for this counter
                // (it's parent's job to filter by counter types)
                var sc = message as SubscribeCounter;
                this.subscriptions.Add(sc.Subscriber);
            }
            else if (message is UnsubscribeCounter)
            {
                // remove a subscription from this counter
                var uc = message as UnsubscribeCounter;
                this.subscriptions.Remove(uc.Subscriber);
            }
        }
    }
}
