using System;

namespace UntoMeWorld.Domain.Errors
{
    public class InvalidQueryFilterException : Exception
    {
        public InvalidQueryFilterException() : base($"Invalid query filter, please refer to documentation")
        {
            
        }
        public InvalidQueryFilterException(string message) : base($"Invalid query filter, please refer to documentation \n${message}")
        {
            
        }
    }
}