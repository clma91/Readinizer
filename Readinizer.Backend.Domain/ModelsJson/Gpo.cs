using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class Gpo
    {
        public int GpoId { get; set; }

        public int RsopRefId { get; set; }

        public Rsop Rsop { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Identifier")]
        public Identifier GpoIdentifier { get; set; } = new Identifier();

        [JsonProperty("Path")]
        public Path GpoPath { get; set; }

        [JsonProperty("Enabled")]
        public string Enabled { get; set; }

        [JsonProperty("Link")]
        public Link Link { get; set; }
    }
}
