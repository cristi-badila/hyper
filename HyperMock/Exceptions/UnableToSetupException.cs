using System;

namespace HyperMock.Universal.Exceptions
{
    public class UnableToSetupException : Exception
    {
        public UnableToSetupException(string expression)
            : base($"Could not perform setup for {expression}")
        {
        }
    }
}