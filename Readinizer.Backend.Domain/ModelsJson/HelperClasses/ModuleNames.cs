using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.HelperClasses
{
    public class ModuleNames
    {
        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("ValueElementData")]
        public string ValueElementData { get; set; }
    }
}
