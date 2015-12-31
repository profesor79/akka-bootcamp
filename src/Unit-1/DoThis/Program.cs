// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinTail
{
    using System;

    using Akka.Actor;

    #region Program

    internal class Program
    {
        /// <summary>
        /// The my actor system.
        /// </summary>
        private static ActorSystem myActorSystem;

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            // initialize MyActorSystem
            myActorSystem = ActorSystem.Create("MyActorSystem");

            // time to make your first actors!
            var consoleWriterActor = myActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()));
            var consoleReaderActor = myActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor)));

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            myActorSystem.AwaitTermination();
        }
    }

    #endregion
}
