using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class RegistrySettingReco
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Path")]
        public string Path { get; set; }

        [JsonProperty("KeyPath")]
        public string KeyPath { get; set; }

        [JsonProperty("TargetValue")]
        public Value TargetValue { get; set; }

        public bool IsPresent { get; set; }

        public Value CurrentValue { get; set; }// = new Value();
    }
}
