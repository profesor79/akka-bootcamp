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
        public InputError(string reason)
        {
            this.Reason = reason;
        }

        public string Reason { get; private set; }
    }
}
