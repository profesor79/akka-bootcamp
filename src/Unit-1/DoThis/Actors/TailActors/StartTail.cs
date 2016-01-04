// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartTail.cs" company="">
//   
// </copyright>
// <summary>
//   Start tailing the file at user-specified path.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinTail.Actors.TailActors
{
    using Akka.Actor;

    /// <summary>
    /// Start tailing the file at user-specified path.
    /// </summary>
    public class StartTail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartTail"/> class.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <param name="reporterActor">
        /// The reporter actor.
        /// </param>
        public StartTail(string filePath, IActorRef reporterActor)
        {
            this.FilePath = filePath;
            this.ReporterActor = reporterActor;
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets the reporter actor.
        /// </summary>
        public IActorRef ReporterActor { get; private set; }
    }
}