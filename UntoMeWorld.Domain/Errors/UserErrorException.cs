using System;

namespace UntoMeWorld.Domain.Errors
{
    public class UserErrorException : Exception
    {
        public string Cause { get; set; }
        public string Fix { get; set; }
        public UserErrorException()
        {
            
        }

        public UserErrorException(string message) : base(message)
        {
            
        }
        
    }
}