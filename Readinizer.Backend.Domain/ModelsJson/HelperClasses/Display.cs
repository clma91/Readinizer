using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.HelperClasses
{
    public class Display
    {
        public Display()
        {
            Name = "Undefined";
            DisplayBoolean = "Undefined";
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("DisplayBoolean")]
        public string DisplayBoolean { get; set; }
    }
}
