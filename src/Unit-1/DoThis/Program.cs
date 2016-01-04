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

    using WinTail.Actors;
    using WinTail.Actors.TailActors;
    using WinTail.Actors.WinTail;

    #region Program

    /// <summary>
    /// The program.
    /// </summary>
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
            // make actor system 
            myActorSystem = ActorSystem.Create("MyActorSystem");

            // create top-level actors within the actor system
            var consoleWriterProps = Props.Create<ConsoleWriterActor>();
            var consoleWriterActor = myActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            var tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            myActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

            var fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor));
            myActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

            var consoleReaderProps = Props.Create<ConsoleReaderActor>();
            var consoleReaderActor = myActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

            // begin processing
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            myActorSystem.AwaitTermination();
        }
    }

    #endregion
}
