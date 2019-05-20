using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Frontend.Messages
{
    public class EnableExport
    {
        public bool ExportEnabled { get; set; }

        public EnableExport()
        {
            ExportEnabled = true;
        }
    }
}
