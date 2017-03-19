using System;

namespace TestApplication.Exceptions
{
    public class ShaderException : Exception
    {
        public ShaderException(string message) : base(message)
        {
        }
    }
}