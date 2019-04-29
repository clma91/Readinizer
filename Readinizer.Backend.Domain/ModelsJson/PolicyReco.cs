using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class PolicyReco
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("State")]
        public string TargetState { get; set; }

        public string CurrentState { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("ModuleNames")]
        public ModuleNames ModuleNames { get; set; }

        public bool IsPresent { get; set; }
    }

    public class ModuleNames
    {
        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("ValueElementData")]
        public string ValueElementData { get; set; }
    }
}
