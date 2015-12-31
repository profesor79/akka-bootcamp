// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleReaderActor.cs" company="">
//   
// </copyright>
// <summary>
//   Actor responsible for reading FROM the console.
//   Also responsible for calling <see cref="ActorSystem.Shutdown" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinTail
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
        private readonly IActorRef consoleWriterActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleReaderActor"/> class.
        /// </summary>
        /// <param name="consoleWriterActor">
        /// The console writer actor.
        /// </param>
        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            this.consoleWriterActor = consoleWriterActor;
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
            else if (message is Messages.ErrorMessages.InputError)
            {
                this.consoleWriterActor.Tell(message as Messages.ErrorMessages.InputError);
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
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }

        /// <summary>
        /// Reads input from console, validates it, then signals appropriate response
        /// (continue processing, error, success, etc.).
        /// </summary>
        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if (string.IsNullOrEmpty(message))
            {
                // signal that the user needs to supply an input, as previously
                // received input was blank
                this.Self.Tell(new Messages.ErrorMessages.NullInputError("No input received."));
            }
            else if (string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // shut down the entire actor system (allows the process to exit)
                Context.System.Shutdown();
            }
            else
            {
                var valid = IsValid(message);
                if (valid)
                {
                    this.consoleWriterActor.Tell(
                        new Messages.SuccessMessages.InputSuccess(
                            "Thank you!\n Message was valid."));

                    // continue reading messages from console
                    this.Self.Tell(new Messages.SystemMessages.ContinueProcessing());
                }
                else
                {
                    this.Self.Tell(
                        new Messages.ErrorMessages.ValidationError(
                            "Invalid: input had \nodd number of characters."));
                }
            }
        }

        
    }
}