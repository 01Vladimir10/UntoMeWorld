using System;

namespace UntoMeWorld.Domain.Errors
{
    public class InvalidQueryFilterException : UserErrorException
    {
        public InvalidQueryFilterException() : base("Invalid query filter, please refer to documentation")
        {
            
        }
        public InvalidQueryFilterException(string message) : base(message)
        {
            Cause = "The query filter provided had an invalid or missing properties.";
            Fix = "Please refer to the documentation on how to use the QueryFilter API correctly";
        }
    }
}