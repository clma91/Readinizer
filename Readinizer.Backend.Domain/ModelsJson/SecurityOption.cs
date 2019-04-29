using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class SecurityOption
    {
        [JsonProperty("GPO")]
        public Gpo Gpo { get; set; }

        [JsonProperty("q3:KeyName")]
        public string KeyName { get; set; }

        [JsonProperty("q3:SettingNumber")]
        public string CurrentSettingNumber { get; set; }

        [JsonProperty("q3:Display")]
        public Display CurrentDisplay { get; set; }
    }

    public class Display
    {
        [JsonProperty("q3:Name")]
        public string Name { get; set; }

        [JsonProperty("q3:DisplayBoolean")]
        public string DisplayBoolean { get; set; }
    }
}
