// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleWriterActor.cs" company="">
//   
// </copyright>
// <summary>
//   Actor responsible for serializing message writes to the console.
//   (write one message at a time, champ :)
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WinTail.Actors
{
    using System;

    using Akka.Actor;

    /// <summary>
    /// Actor responsible for serializing message writes to the console.
    /// (write one message at a time, champ :)
    /// </summary>
    public class ConsoleWriterActor : UntypedActor
    {
        /// <summary>
        /// The on receive.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void OnReceive(object message)
        {
            if (message is Messages.ErrorMessages.InputError)
            {
                var msg = message as Messages.ErrorMessages.InputError;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg.Reason);
            }
            else if (message is Messages.SuccessMessages.InputSuccess)
            {
                var msg = message as Messages.SuccessMessages.InputSuccess;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(msg.Reason);
            }
            else
            {
                Console.WriteLine(message);
            }

            Console.ResetColor();
        }
    }
}
