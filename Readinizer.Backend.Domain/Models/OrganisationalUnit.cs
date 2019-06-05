using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.Models
{
    public class OrganisationalUnit
    {
        [JsonIgnore]
        public int OrganisationalUnitId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public string LdapPath { get; set; }

        [JsonIgnore]
        public int? ParentId { get; set; }

        [JsonIgnore]
        public int ADDomainRefId { get; set; }

        public virtual ADDomain ADDomain { get; set; }

        [JsonIgnore]
        public virtual List<Rsop> Rsops { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrganisationalUnit> SubOrganisationalUnits { get; set; }

        [JsonIgnore]
        public virtual ICollection<Computer> Computers { get; set; }

        [JsonIgnore]
        public bool? HasReachableComputer { get; set; }
    }
}
