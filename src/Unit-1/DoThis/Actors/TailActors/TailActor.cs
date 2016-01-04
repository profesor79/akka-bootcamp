// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TailActor.cs" company="">
//   
// </copyright>
// <summary>
//   Monitors the file at  for changes and sends
//   file updates to console.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WinTail.Actors.TailActors
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Text;

    using Akka.Actor;

    /// <summary>
    /// Monitors the file at <see cref="filePath"/> for changes and sends
    /// file updates to console.
    /// </summary>
    public class TailActor : UntypedActor
    {
        /// <summary>
        /// The _file path.
        /// </summary>
        private readonly string filePath;

        /// <summary>
        /// The _reporter actor.
        /// </summary>
        private readonly IActorRef reporterActor;

        /// <summary>
        /// The _observer.
        /// </summary>
        private readonly FileObserver observer;

        /// <summary>
        /// The _file stream.
        /// </summary>
        private readonly Stream fileStream;

        /// <summary>
        /// The _file stream reader.
        /// </summary>
        private readonly StreamReader fileStreamReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="TailActor"/> class.
        /// </summary>
        /// <param name="reporterActor">
        /// The reporter actor.
        /// </param>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        public TailActor(IActorRef reporterActor, string filePath)
        {
            Contract.Requires(reporterActor != null);
            Contract.Requires(filePath != null);
            this.reporterActor = reporterActor;
            this.filePath = filePath;

            // start watching file for changes
            this.observer = new FileObserver(this.Self, Path.GetFullPath(this.filePath));
            this.observer.Start();

            // open the file stream with shared read/write permissions
            // (so file can be written to while open)
            this.fileStream = new FileStream(
                Path.GetFullPath(this.filePath), 
                FileMode.Open, 
                FileAccess.Read, 
                FileShare.ReadWrite);
            this.fileStreamReader = new StreamReader(this.fileStream, Encoding.UTF8);

            // read the initial contents of the file and send it to console as first msg
            var text = this.fileStreamReader.ReadToEnd();
            this.Self.Tell(new InitialRead(this.filePath, text));
        }

        /// <summary>
        /// The on receive.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void OnReceive(object message)
        {
            if (message is FileWrite)
            {
                // move file cursor forward
                // pull results from cursor to end of file and write to output
                // (this is assuming a log file type format that is append-only)
                var text = this.fileStreamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(text))
                {
                    this.reporterActor.Tell(text);
                }

            }
            else if (message is FileError)
            {
                var fe = message as FileError;
                this.reporterActor.Tell(string.Format("Tail error: {0}", fe.Reason));
            }
            else if (message is InitialRead)
            {
                var ir = message as InitialRead;
                this.reporterActor.Tell(ir.Text);
            }
        }
    }
}