using System;

namespace GameStore.BusinessLayer.Exceptions
{
    public class InvalidServiceOperationException : InvalidOperationException
    {
        public string ExceptionMessage { get; }

        public InvalidServiceOperationException(string message)
            : base(message)
        {
            ExceptionMessage = message;
        }
    }
}