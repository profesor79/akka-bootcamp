// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitialRead.cs" company="">
//   
// </copyright>
// <summary>
//   Signal to read the initial contents of the file at actor startup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WinTail.Actors.TailActors
{
    /// <summary>
    /// Signal to read the initial contents of the file at actor startup.
    /// </summary>
    public class InitialRead
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitialRead"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        public InitialRead(string fileName, string text)
        {
            this.FileName = fileName;
            this.Text = text;
        }

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text { get; private set; }
    }
}