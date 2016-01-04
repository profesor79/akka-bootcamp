// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileError.cs" company="">
//   
// </copyright>
// <summary>
//   Signal that the OS had an error accessing the file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WinTail.Actors.TailActors
{
    /// <summary>
    /// Signal that the OS had an error accessing the file.
    /// </summary>
    public class FileError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileError"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="reason">
        /// The reason.
        /// </param>
        public FileError(string fileName, string reason)
        {
            this.FileName = fileName;
            this.Reason = reason;
        }

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the reason.
        /// </summary>
        public string Reason { get; private set; }
    }
}