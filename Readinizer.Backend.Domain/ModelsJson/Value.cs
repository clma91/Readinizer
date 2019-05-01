using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class Value
    {
        [JsonProperty("Element")]
        public Element Element { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Number")]
        public string Number { get; set; }
    }

    public class Element
    {
        [JsonProperty("Data")]
        public string Modules { get; set; }
    }
}
