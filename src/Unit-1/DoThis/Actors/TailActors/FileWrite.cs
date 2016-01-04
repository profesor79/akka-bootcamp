// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileWrite.cs" company="">
//   
// </copyright>
// <summary>
//   Signal that the file has changed, and we need to read the next line of the file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WinTail.Actors.TailActors
{
    /// <summary>
    /// Signal that the file has changed, and we need to read the next line of the file.
    /// </summary>
    public class FileWrite
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileWrite"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        public FileWrite(string fileName)
        {
            this.FileName = fileName;
        }

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName { get; private set; }
    }
}