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
            // initialize MyActorSystem
            myActorSystem = ActorSystem.Create("MyActorSystem");

            // time to make your first actors!
            var consoleWriterProps = Props.Create<ConsoleWriterActor>();
            var consoleWriterActor = myActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            var tailCoordinatingProps = Props.Create(() => new TailCoordinatorActor());
            var tailCoordinatingActor = myActorSystem.ActorOf(tailCoordinatingProps, "tailCoordinator");
            
            var validationActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor, tailCoordinatingActor));
            var validationActor = myActorSystem.ActorOf(validationActorProps, "validationActor");

            var consoleReaderProps = Props.Create<ConsoleReaderActor>(validationActor);
            var consoleReaderActor = myActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            myActorSystem.AwaitTermination();
        }
    }

    #endregion
}
