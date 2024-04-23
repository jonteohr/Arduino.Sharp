using System;

namespace Arduino.Sharp.Exceptions
{
    public class ConnectivityException : Exception
    {
        public ConnectivityException()
        {
        }

        public ConnectivityException(string message) : base(message)
        {
        }
        
        public ConnectivityException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}