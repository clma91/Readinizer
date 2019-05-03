using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class PolicyJson
    {
        [JsonProperty("GPO")]
        public Gpo Gpo { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("State")]
        public string CurrentState { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("Listbox")]
        public ListBox ModuleNames { get; set; }
    }
}
