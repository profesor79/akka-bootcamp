// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputError.cs" company="">
//   
// </copyright>
// <summary>
//   The input error.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WinTail.Messages.ErrorMessages
{
    /// <summary>
    /// The input error.
    /// </summary>
    public class InputError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputError"/> class.
        /// </summary>
        /// <param name="reason">
        /// The reason.
        /// </param>
        public InputError(string reason)
        {
            this.Reason = reason;
        }

        /// <summary>
        /// Gets the reason.
        /// </summary>
        public string Reason { get; private set; }
    }
}
