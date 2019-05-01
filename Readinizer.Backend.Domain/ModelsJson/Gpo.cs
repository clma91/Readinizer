using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class Gpo
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Identifier")]
        public Identifier GpoIdentifier { get; set; }

        [JsonProperty("Path")]
        public Path GpoPath { get; set; }

        [JsonProperty("Enabled")]
        public string Enabled { get; set; }

        [JsonProperty("Link")]
        public Link Link { get; set; }
    }

    public class Identifier
    {
        [JsonProperty("#text")]
        public string Id { get; set; }
    }

    public class Path
    {
        [JsonProperty("Identifier")]
        public Identifier GpoIdentifier { get; set; }
    }

    public class Link
    {
        [JsonProperty("SOMPath")]
        public string SOMPath { get; set; }

        [JsonProperty("AppliedOrder")]
        public string AppliedOrder { get; set; }
    }
}
