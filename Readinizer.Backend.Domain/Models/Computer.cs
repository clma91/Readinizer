using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.Models
{
    public class Computer
    {
        [JsonIgnore]
        public int ComputerId { get; set; }

        public string ComputerName { get; set; }

        public bool IsDomainController { get; set; }

        public virtual ICollection<OrganisationalUnit> OrganisationalUnits { get; set; }

        public string IpAddress { get; set; }

        [JsonIgnore]
        public bool PingSuccessful { get; set; }

        [JsonIgnore]
        public int SiteRefId { get; set; }

        public Site Site { get; set; }

        public bool? isSysmonRunning { get; set; }
    }
}
