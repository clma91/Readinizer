using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class AuditSetting
    {
        [JsonProperty("GPO")]
        public Gpo Gpo { get; set; }

        [JsonProperty("q2:SubCategoryName")]
        public string SubcategoryName { get; set; }

        [JsonProperty("q2:PolicyTarget")]
        public string PolicyTarget { get; set; }

        [JsonProperty("q2:SettingValue")]
        public AuditSettingValue CurrentSettingValue { get; set; }
    }

    public enum AuditSettingValue
    {
        NoAuditing,
        Success,
        Failure,
        SuccessAndFailure
    }
}
