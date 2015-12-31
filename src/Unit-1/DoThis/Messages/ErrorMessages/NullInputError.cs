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
        public NullInputError(string reason)
            : base(reason)
        {
        }
    }

  
}
