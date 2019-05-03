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
        
        public virtual ICollection<AuditSetting> AuditSettings { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }

        public virtual ICollection<RegistrySetting> RegistrySettings { get; set; }

        public virtual ICollection<SecurityOption> SecurityOptions { get; set; }

        public virtual ICollection<Gpo> Gpos { get; set; }
    }
}
