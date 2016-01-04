// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationError.cs" company="">
//   
// </copyright>
// <summary>
//   The validation error.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WinTail.Messages.ErrorMessages
{
    /// <summary>
    /// The validation error.
    /// </summary>
    public class ValidationError:InputError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="reason">
        /// The reason.
        /// </param>
        public ValidationError(string reason)
            : base(reason)
        {
        }
    }
}
