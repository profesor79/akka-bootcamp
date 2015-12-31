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
        public ValidationError(string reason)
            : base(reason)
        {
        }
    }
}
