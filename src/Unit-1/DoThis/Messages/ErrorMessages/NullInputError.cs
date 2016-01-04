// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullInputError.cs" company="">
//   
// </copyright>
// <summary>
//   The null input error.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace WinTail.Messages.ErrorMessages
{
    /// <summary>
    /// The null input error.
    /// </summary>
    public class NullInputError:InputError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullInputError"/> class.
        /// </summary>
        /// <param name="reason">
        /// The reason.
        /// </param>
        public NullInputError(string reason)
            : base(reason)
        {
        }
    }

  
}
