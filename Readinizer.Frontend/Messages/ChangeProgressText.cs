using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Frontend.Messages
{
    public class ChangeProgressText
    {
        public string ProgressText { get; set; }

        public ChangeProgressText(string progressText)
        {
            ProgressText = progressText;
        }
    }
}
