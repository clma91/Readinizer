using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class ADOrganisationalUnit
    {
        public int ADOrganisationalUnitId { get; set; }

        public string Name { get; set; }

        public string LdapPath { get; set; }

        public int? ParentId { get; set; }

        public int ADDomainRefId { get; set; }

        public ADDomain ADDomain { get; set; }

        public List<ADOrganisationalUnit> SubADOrganisationalUnits { get; set; }

        public List<ADOuMember> ADOuMembers { get; set; }

        public bool HasReachableComputer { get; set; }
    }
}
