using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class SecuritySettingsParserOU : SecuritySettingsParsed
    {
        public string GPO { get; set; }
    }
}
