using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Frontend.Messages
{
    public class SnackbarMessage
    {
        public string Message { get; }

        public SnackbarMessage(string message)
        {
            Message = message;
        }
    }
}
