// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationActor.cs" company="">
//   
// </copyright>
// <summary>
//   The validation actor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WinTail.Actors
{
    using Akka.Actor;

    /// <summary>
    /// The validation actor.
    /// </summary>
    public class ValidationActor : UntypedActor
    {
        /// <summary>
        /// The console writer actor.
        /// </summary>
        private readonly IActorRef consoleWriterActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationActor"/> class.
        /// </summary>
        /// <param name="consoleWriterActor">
        /// The console writer actor.
        /// </param>
        public ValidationActor(IActorRef consoleWriterActor)
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
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                // signal that the user needs to supply an input
                this.consoleWriterActor.Tell(new Messages.ErrorMessages.NullInputError("No input received."));
            }
            else
            {
                var valid = IsValid(msg);
                if (valid)
                {
                    // send success to console writer
                    this.consoleWriterActor.Tell(new Messages.SuccessMessages.InputSuccess("Thank you! Message was valid."));
                }
                else
                {
                    // signal that input was bad
                    this.consoleWriterActor.Tell(new Messages.ErrorMessages.ValidationError("Invalid: input had odd number of characters."));
                }
            }

            // tell sender to continue doing its thing
            // (whatever that may be, this actor doesn't care)
            this.Sender.Tell(new Messages.SystemMessages.ContinueProcessing());
        }

        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsValid(string msg)
        {
            var valid = msg.Length % 2 == 0;
            return valid;
        }
    }
}
