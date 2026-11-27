using System;
using System.Collections.Generic;
namespace Final_Project.Exceptions
{
    public class InvalidSecretWordException : Exception
    {
        public InvalidSecretWordException(string message) : base(message)
        {
        }
    }
}