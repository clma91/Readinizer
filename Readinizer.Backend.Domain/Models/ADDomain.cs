using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.Models
{
    public class ADDomain
    {
        [JsonIgnore]
        public int ADDomainId { get; set; }

        [JsonIgnore]
        public int? ParentId { get; set; }

        public string Name { get; set; }

        public bool IsTreeRoot { get; set; }

        public bool IsForestRoot { get; set; }

        public bool IsAvailable { get; set; }

        [JsonIgnore]
        public virtual List<ADDomain> SubADDomains { get; set; }

        [JsonIgnore]
        public virtual List<OrganisationalUnit> OrganisationalUnits { get; set; }

        [JsonIgnore]
        public virtual ICollection<Site> Sites { get; set; }

        [JsonIgnore]
        public virtual List<Rsop> Rsops { get; set; }

        [JsonIgnore]
        public virtual List<RsopPot> RsopPots { get; set; }

        public double? RsopsPercentage { get; set; }
    }
}
