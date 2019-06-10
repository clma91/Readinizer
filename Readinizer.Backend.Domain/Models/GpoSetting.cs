using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class GpoSetting
    {
        public string GpoIdentifier { get; set; }

        public bool IsPresent { get; set; }

        public virtual bool IsStatusOk { get; set; }
    }
}
