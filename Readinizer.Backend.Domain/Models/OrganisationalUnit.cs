using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class OrganisationalUnit
    {
        public int OrganisationalUnitId { get; set; }

        public string Name { get; set; }

        public string LdapPath { get; set; }

        public int? ParentId { get; set; }

        public int ADDomainRefId { get; set; }

        public ADDomain ADDomain { get; set; }

        public ICollection<OrganisationalUnit> SubOrganisationalUnits { get; set; }

        public ICollection<Computer> Computers { get; set; }

        public bool HasReachableComputer { get; set; }
    }
}
