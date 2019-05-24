using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class AuditSetting
    {
        [JsonIgnore]
        public int AuditSettingId { get; set; }

        [JsonIgnore]
        public int RsopRefId { get; set; }

        [JsonIgnore]
        public Rsop Rsop { get; set; }

        public string GpoId { get; set; }

        [JsonProperty("SubCategoryName")]
        public string SubcategoryName { get; set; }

        [JsonProperty("PolicyTarget")]
        public string PolicyTarget { get; set; }

        [JsonProperty("TargetSettingValue")]
        public AuditSettingValue TargetSettingValue { get; set; }

        [JsonProperty("SettingValue")]
        public AuditSettingValue CurrentSettingValue { get; set; }

        public bool IsPresent { get; set; }

        public bool IsStatusOk => CurrentSettingValue.Equals(TargetSettingValue);

        public override bool Equals(object obj)
        {
            if (GpoId != null && SubcategoryName != null)
            {
                var auditSetting = obj as AuditSetting;

                if (auditSetting == null)
                {
                    return false;
                }

                return SubcategoryName.Equals(auditSetting.SubcategoryName) && CurrentSettingValue.Equals(auditSetting.CurrentSettingValue);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
