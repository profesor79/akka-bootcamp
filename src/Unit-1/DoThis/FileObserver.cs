// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileObserver.cs" company="">
//   
// </copyright>
// <summary>
//   FileObserver.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WinTail
{
    using System;
    using System.IO;

    using Akka.Actor;

    using WinTail.Actors.TailActors;

    /// <summary>
    /// Turns <see cref="FileSystemWatcher"/> events about a specific file into
    /// messages for <see cref="TailActor"/>.
    /// </summary>
    public class FileObserver : IDisposable
    {
        /// <summary>
        /// The _tail actor.
        /// </summary>
        private readonly IActorRef tailActor;

        /// <summary>
        /// The _absolute file path.
        /// </summary>
        private readonly string absoluteFilePath;

        /// <summary>
        /// The _watcher.
        /// </summary>
        private FileSystemWatcher watcher;

        /// <summary>
        /// The _file dir.
        /// </summary>
        private readonly string fileDir;

        /// <summary>
        /// The _file name only.
        /// </summary>
        private readonly string fileNameOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileObserver"/> class.
        /// </summary>
        /// <param name="tailActor">
        /// The tail actor.
        /// </param>
        /// <param name="absoluteFilePath">
        /// The absolute file path.
        /// </param>
        public FileObserver(IActorRef tailActor, string absoluteFilePath)
        {
            this.tailActor = tailActor;
            this.absoluteFilePath = absoluteFilePath;
            this.fileDir = Path.GetDirectoryName(absoluteFilePath);
            this.fileNameOnly = Path.GetFileName(absoluteFilePath);
        }

        /// <summary>
        /// Begin monitoring file.
        /// </summary>
        public void Start()
        {
            // make watcher to observe our specific file
            this.watcher = new FileSystemWatcher(this.fileDir, this.fileNameOnly)
                               {
                                   NotifyFilter =
                                       NotifyFilters.FileName
                                       | NotifyFilters.LastWrite
                               };

            // watch our file for changes to the file name,
            // or new messages being written to file

            // assign callbacks for event types
            this.watcher.Changed += this.OnFileChanged;
            this.watcher.Error += this.OnFileError;

            // start watching
            this.watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Stop monitoring file.
        /// </summary>
        public void Dispose()
        {
            this.watcher.Dispose();
        }

        /// <summary>
        /// Callback for <see cref="FileSystemWatcher"/> file error events.
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void OnFileError(object sender, ErrorEventArgs e)
        {
            this.tailActor.Tell(new FileError(this.fileNameOnly, e.GetException().Message), ActorRefs.NoSender);
        }

        /// <summary>
        /// Callback for <see cref="FileSystemWatcher"/> file change events.
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                // here we use a special ActorRefs.NoSender
                // since this event can happen many times,
                // this is a little microoptimization
                this.tailActor.Tell(new FileWrite(e.Name), ActorRefs.NoSender);
            }
        }
    }
}
