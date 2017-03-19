using System;

namespace TestApplication.Exceptions
{
    public class MaterialException : Exception
    {
        public MaterialException(string message) : base(message)
        {
        }
    }
}