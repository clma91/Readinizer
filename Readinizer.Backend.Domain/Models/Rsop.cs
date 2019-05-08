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

        public OrganisationalUnit OrganisationalUnit { get; set; }

        public Site Site { get; set; }

        public int? RsopPotRefId { get; set; }

        public virtual RsopPot RsopPot { get; set; }

        public virtual ICollection<AuditSetting> AuditSettings { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }

        public virtual ICollection<RegistrySetting> RegistrySettings { get; set; }

        public virtual ICollection<SecurityOption> SecurityOptions { get; set; }

        public virtual ICollection<Gpo> Gpos { get; set; }

        public double RsopPercentage => Math.Round((AuditSettingPercentage + PoliciesPercentage + RegistrySettingsPercentage + SecurityOptionsPercentage) / 4);

        public double AuditSettingPercentage
        {
            get
            {
                var counter = AuditSettings.Count(auditSetting => auditSetting.TargetSettingValue == auditSetting.CurrentSettingValue);
                return Math.Round((double)(100 / AuditSettings.Count) * counter);
            }
        }

        public double PoliciesPercentage
        {
            get
            {
                var counter = Policies.Count(policy => policy.TargetState == policy.CurrentState);
                return Math.Round((double)(100 / Policies.Count) * counter);
            }
        }

        public double RegistrySettingsPercentage
        {
            get
            {
                var counter = RegistrySettings.Count(registrySetting => registrySetting.TargetValue.Element.Modules == registrySetting.CurrentValue.Element.Modules);
                return Math.Round((double)(100 / RegistrySettings.Count) * counter);
            }
        }

        public double SecurityOptionsPercentage
        {
            get
            {
                var counter = SecurityOptions.Count(securityOption => securityOption.TargetDisplay.DisplayBoolean == securityOption.CurrentDisplay.DisplayBoolean);
                return Math.Round((double)(100 / SecurityOptions.Count) * counter);
            }
        }
    }
}
