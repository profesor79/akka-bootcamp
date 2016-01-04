// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StopTail.cs" company="">
//   
// </copyright>
// <summary>
//   Stop tailing the file at user-specified path.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WinTail.Actors.TailActors
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Stop tailing the file at user-specified path.
    /// </summary>
    public class StopTail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopTail"/> class.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        public StopTail(string filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// The object invariant.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.FilePath != null);
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        public string FilePath { get; private set; }
    }
}