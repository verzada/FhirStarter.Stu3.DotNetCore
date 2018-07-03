using System;

namespace FhirStarter.Bonfire.STU3.DotNetCore.Exceptions
{
    public class ValidateInputException : ArgumentException
    {
        public ValidateInputException(string message) : base(message)
        {
        }

        public ValidateInputException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
