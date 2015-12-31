// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TailCoordinatorActor.cs" company="">
//   
// </copyright>
// <summary>
//   The tail coordinator actor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinTail.Actors.TailActors
{
    using System;

    using Akka.Actor;

    /// <summary>
    /// The tail coordinator actor.
    /// </summary>
    public class TailCoordinatorActor : UntypedActor
    {
        /// <summary>
        /// The on receive.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void OnReceive(object message)
        {
            if (message is StartTail)
            {
                var msg = message as StartTail;

                // here we are creating our first parent/child relationship!
                // the TailActor instance created here is a child
                // of this instance of TailCoordinatorActor
                Context.ActorOf(Props.Create(() => new TailActor(msg.ReporterActor, msg.FilePath)));
            }
        }

        /// <summary>
        /// The supervisor strategy.
        /// </summary>
        /// <returns>
        /// The <see cref="SupervisorStrategy"/>.
        /// </returns>
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                10, // maxNumberOfRetries
                TimeSpan.FromSeconds(30), // withinTimeRange
                x => // localOnlyDecider
                {
                    // Maybe we consider ArithmeticException to not be application critical
                    // so we just ignore the error and keep going.
                    if (x is ArithmeticException) return Directive.Resume;

                    // Error that we cannot recover from, stop the failing actor
                    if (x is NotSupportedException) return Directive.Stop;

                    // In all other cases, just restart the failing actor
                    return Directive.Restart;
                });
        }
    }
}
