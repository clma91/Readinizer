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
        [JsonProperty("q7:Element")]
        public Element Element { get; set; }

        [JsonProperty("q7:Name")]
        public string Name { get; set; }

        [JsonProperty("q7:Number")]
        public string Number { get; set; }
    }

    public class Element
    {
        [JsonProperty("q7:Data")]
        public string Modules { get; set; }
    }
}
