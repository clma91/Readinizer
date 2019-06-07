using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Exceptions
{
    public class InvalidXmlException : Exception
    {
        public string Details { get; set; }

        public InvalidXmlException(string message, string details = null) : base(message)
        {
            Details = details;
        }
    }
}
