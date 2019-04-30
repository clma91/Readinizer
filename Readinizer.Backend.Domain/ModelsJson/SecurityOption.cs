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

        [JsonProperty("KeyName")]
        public string KeyName { get; set; }

        [JsonProperty("SettingNumber")]
        public string CurrentSettingNumber { get; set; }

        [JsonProperty("Display")]
        public Display CurrentDisplay { get; set; }
    }

    public class Display
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("DisplayBoolean")]
        public string DisplayBoolean { get; set; }
    }
}
