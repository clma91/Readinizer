using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Exceptions
{
    public class InvalidAuthenticationException : Exception
    {
        public string Details { get; set; } 

        public InvalidAuthenticationException(string message, string details = null) : base(message)
        {
            Details = details;
        }
    }
}
