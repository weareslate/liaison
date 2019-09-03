using System;
using System.Collections.Generic;

namespace Liaison.AspNetCore.Exceptions
{
    public class BadRequestException 
        : Exception
    {
        public BadRequestException(
            string message,
            Dictionary<string, string> errors = null )
            : base( message )
        {
            this.Errors = errors ?? new Dictionary<string, string>();
        }
        
        public Dictionary<string, string> Errors { get; }
    }
}