using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class Policy
    {
        [JsonProperty("GPO")]
        public Gpo Gpo { get; set; }

        [JsonProperty("q7:Name")]
        public string Name { get; set; }

        [JsonProperty("q7:State")]
        public string CurrentState { get; set; }

        [JsonProperty("q7:Category")]
        public string Category { get; set; }

        [JsonProperty("q7:Listbox")]
        public ListBox ModuleNames { get; set; }
    }

    public class ListBox
    {
        [JsonProperty("q7:Value")]
        public Value Value { get; set; }

        [JsonProperty("q7:State")]
        public string State { get; set; }
    }
}
