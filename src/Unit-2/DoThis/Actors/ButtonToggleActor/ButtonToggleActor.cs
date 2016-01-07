// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ButtonToggleActor.cs" company="">
//   
// </copyright>
// <summary>
//   Actor responsible for managing button toggles
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Copyright

// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ButtonToggleActor.cs" company="Glass Lewis">
//  All rights reserved @2015
//  </copyright>
//  <summary>
//  </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace ChartApp.Actors.ButtonToggleActor
{
    #region Usings

    using System.Windows.Forms;

    using Akka.Actor;

    using ChartApp.Enums;

    #endregion

    /// <summary>
    ///     Actor responsible for managing button toggles
    /// </summary>
    public class ButtonToggleActor : UntypedActor
    {
        /// <summary>
        ///     The _coordinator actor.
        /// </summary>
        private readonly IActorRef coordinatorActor;

        /// <summary>
        ///     The _my button.
        /// </summary>
        private readonly Button myButton;

        /// <summary>
        ///     The _my counter type.
        /// </summary>
        private readonly CounterType myCounterType;

        /// <summary>
        ///     The _is toggled on.
        /// </summary>
        private bool isToggledOn;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonToggleActor"/> class.
        /// </summary>
        /// <param name="coordinatorActor">
        /// The coordinator actor.
        /// </param>
        /// <param name="myButton">
        /// The my button.
        /// </param>
        /// <param name="myCounterType">
        /// The my counter type.
        /// </param>
        /// <param name="isToggledOn">
        /// The is toggled on.
        /// </param>
        public ButtonToggleActor(
            IActorRef coordinatorActor, 
            Button myButton, 
            CounterType myCounterType, 
            bool isToggledOn = false)
        {
            this.coordinatorActor = coordinatorActor;
            this.myButton = myButton;
            this.isToggledOn = isToggledOn;
            this.myCounterType = myCounterType;
        }

        /// <summary>
        /// The on receive.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void OnReceive(object message)
        {
            if (message is Toggle && this.isToggledOn)
            {
                // toggle is currently on

                // stop watching this counter
                this.coordinatorActor.Tell(new PerformanceCounterCoordinatorActor.Unwatch(this.myCounterType));

                this.FlipToggle();
            }
            else if (message is Toggle && !this.isToggledOn)
            {
                // toggle is currently off

                // start watching this counter
                this.coordinatorActor.Tell(new PerformanceCounterCoordinatorActor.Watch(this.myCounterType));

                this.FlipToggle();
            }
            else
            {
                this.Unhandled(message);
            }
        }

        /// <summary>
        ///     The flip toggle.
        /// </summary>
        private void FlipToggle()
        {
            // flip the toggle
            this.isToggledOn = !this.isToggledOn;

            // change the text of the button
            this.myButton.Text = string.Format(
                "{0} ({1})", 
                this.myCounterType.ToString().ToUpperInvariant(), 
                this.isToggledOn ? "ON" : "OFF");
        }
    }
}