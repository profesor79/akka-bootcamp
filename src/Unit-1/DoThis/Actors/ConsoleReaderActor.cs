// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleReaderActor.cs" company="">
//   
// </copyright>
// <summary>
//   Actor responsible for reading FROM the console.
//   Also responsible for calling <see cref="ActorSystem.Shutdown" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinTail.Actors
{
    using System;

    using Akka.Actor;

    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Shutdown"/>.
    /// </summary>
    public class ConsoleReaderActor : UntypedActor
    {
        /// <summary>
        /// The start command.
        /// </summary>
        public const string StartCommand = "start";

        /// <summary>
        /// The exit command.
        /// </summary>
        public const string ExitCommand = "exit";

        /// <summary>
        /// The console writer actor.
        /// </summary>
        private readonly IActorRef validationActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleReaderActor"/> class.
        /// </summary>
        /// <param name="validationActor">
        /// The console writer actor.
        /// </param>
        public ConsoleReaderActor(IActorRef validationActor)
        {
            this.validationActor = validationActor;
        }

        /// <summary>
        /// The on receive.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }

            this.GetAndValidateInput();
        }

        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValid(string message)
        {
            var valid = message.Length % 2 == 0;
            return valid;
        }

        /// <summary>
        /// The do print instructions.
        /// </summary>
        private static void DoPrintInstructions()
        {
            Console.WriteLine("Please provide the URI of a log file on disk.\n");
        }

        /// <summary>
        /// Reads input from console, validates it, then signals appropriate response
        /// (continue processing, error, success, etc.).
        /// </summary>
        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if (!string.IsNullOrEmpty(message) &&
            string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // if user typed ExitCommand, shut down the entire actor
                // system (allows the process to exit)
                Context.System.Shutdown();
                return;
            }

            // otherwise, just hand message off to validation actor
            // (by telling its actor ref)
            validationActor.Tell(message);
        }

        
    }
}