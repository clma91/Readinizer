using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson.HelperClasses
{
    public class Path
    {
        [JsonProperty("Identifier")]
        public Identifier GpoIdentifier { get; set; }
    }
}
