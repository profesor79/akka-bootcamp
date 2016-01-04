// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileValidatorActor.cs" company="">
//   
// </copyright>
// <summary>
//   FileValidatorActor.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinTail.Actors
{
    using System.IO;

    using Akka.Actor;

    namespace WinTail
{
    using global::WinTail.Actors.TailActors;

    /// <summary>
    /// Actor that validates user input and signals result to others.
    /// </summary>
    public class FileValidatorActor : UntypedActor
    {
        /// <summary>
        /// The console writer actor.
        /// </summary>
        private readonly IActorRef consoleWriterActor;

        public FileValidatorActor(IActorRef consoleWriterActor )
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
                this.consoleWriterActor.Tell(new Messages.ErrorMessages.NullInputError("Input was blank.Please try again.\n"));

                // tell sender to continue doing its thing (whatever that may be,
                // this actor doesn't care)
                this.Sender.Tell(new Messages.SystemMessages.ContinueProcessing());
            }
            else
            {
                var valid = IsFileUri(msg);
                if (valid)
                {
                    // signal successful input
                    this.consoleWriterActor.Tell(new Messages.SuccessMessages.InputSuccess(
                        string.Format("Starting processing for {0}", msg)));

                    // start coordinator
                    Context.ActorSelection("akka://MyActorSystem/user/tailCoordinatorActor").Tell(new StartTail(msg, this.consoleWriterActor));
                }
                else
                {
                    // signal that input was bad
                    this.consoleWriterActor.Tell(new Messages.ErrorMessages.ValidationError(
                        string.Format("{0} is not an existing URI on disk.", msg)));

                    // tell sender to continue doing its thing (whatever that
                    // may be, this actor doesn't care)
                    this.Sender.Tell(new Messages.SystemMessages.ContinueProcessing());
                }
            }



        }

        /// <summary>
        /// The is file uri.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsFileUri(string path)
        {
            return File.Exists(path);
        }
    }
}
}
