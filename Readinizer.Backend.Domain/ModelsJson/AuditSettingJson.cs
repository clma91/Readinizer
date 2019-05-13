using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class AuditSettingJson
    {
        public AuditSettingJson()
        {
            SubcategoryName = "Undefined";
            PolicyTarget = "Undefined";
            CurrentSettingValue = 0;
        }

        [JsonProperty("GPO")]
        public Gpo Gpo { get; set; }

        [JsonProperty("SubCategoryName")]
        public string SubcategoryName { get; set; }

        [JsonProperty("PolicyTarget")]
        public string PolicyTarget { get; set; }

        [JsonProperty("SettingValue")]
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
