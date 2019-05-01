using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class RegistrySetting
    {
        [JsonProperty("GPO")]
        public Gpo Gpo { get; set; }

        [JsonProperty("KeyPath")]
        public string KeyPath { get; set; }

        [JsonProperty("Value")]
        public Value CurrentValue { get; set; }
    }
}
