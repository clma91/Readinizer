using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.Domain.ModelsJson;

namespace Readinizer.Backend.Domain.Models
{
    public class Rsop
    {
        public int RsopId { get; set; }

        public string OrganisationalUnitName { get; set; }

        public List<AuditSettingReco> AuditSettings { get; set; }

        public List<PolicyReco> Policies { get; set; }

        public List<RegistrySettingReco> RegistrySettings { get; set; }

        public List<SecurityOptionReco> SecurityOptions { get; set; }

        public List<Gpo> Gpos { get; set; }
    }
}
