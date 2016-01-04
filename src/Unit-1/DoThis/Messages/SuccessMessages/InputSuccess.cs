// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputSuccess.cs" company="">
//   
// </copyright>
// <summary>
//   The input success.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WinTail.Messages.SuccessMessages
{
    /// <summary>
    /// The input success.
    /// </summary>
    public class InputSuccess
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputSuccess"/> class.
        /// </summary>
        /// <param name="reason">
        /// The reason.
        /// </param>
        public InputSuccess(string reason)
        {
            this.Reason = reason;
        }

        /// <summary>
        /// Gets the reason.
        /// </summary>
        public string Reason { get; private set; }
    }
}
