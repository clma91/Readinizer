using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class RsopPot
    {
        public int RsopPotId { get; set; }

        public string Name { get; set; }

        //public List<OrganisationalUnit> OrganisationalUnits { get; set; }

        //public Site Site { get; set; }

        public virtual ICollection<Rsop> Rsops { get; set; }
    }
}
