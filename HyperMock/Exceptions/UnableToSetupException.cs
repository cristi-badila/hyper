namespace HyperMock.Universal.Exceptions
{
    using System;

    public class UnableToSetupException : Exception
    {
        public UnableToSetupException(string expression)
            : base($"Could not perform setup for {expression}")
        {
        }
    }
}