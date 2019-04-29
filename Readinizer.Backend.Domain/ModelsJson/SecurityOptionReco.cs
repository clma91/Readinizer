using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class SecurityOptionReco
    {
        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Path")]
        public string Path { get; set; }

        [JsonProperty("KeyName")]
        public string KeyName { get; set; }

        [JsonProperty("SettingNumber")]
        public string TargetSettingNumber { get; set; }

        [JsonProperty("q3:Display")]
        public Display TargetDisplay { get; set; }

        public bool IsPresent { get; set; }

        public Display CurrentDisplay { get; set; }

        public string CurrentSettingNumber { get; set; }
    }
}
