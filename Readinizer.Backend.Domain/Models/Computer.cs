using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class Computer
    {
        public int ComputerId { get; set; }

        public string ComputerName { get; set; }

        public bool IsDomainController { get; set; }

        public virtual ICollection<OrganisationalUnit> OrganisationalUnits { get; set; }

        public string IpAddress { get; set; }

        public bool PingSuccessfull { get; set; }

        public int SiteRefId { get; set; }

        public Site Site { get; set; }
    }
}
