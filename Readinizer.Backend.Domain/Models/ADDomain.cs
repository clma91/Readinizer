using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class ADDomain
    {
        public int ADDomainId { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public bool IsTreeRoot { get; set; }

        public bool IsForestRoot { get; set; }

        public virtual List<ADDomain> SubADDomains { get; set; }

        public virtual List<OrganisationalUnit> OrganisationalUnits { get; set; }

        public virtual ICollection<Site> Sites { get; set; }
    }
}
