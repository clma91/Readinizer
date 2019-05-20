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
    public class Policy 
    {
        [JsonIgnore]
        public int PolicyId { get; set; }

        [JsonIgnore]
        public int RsopRefId { get; set; }

        [JsonIgnore]
        public Rsop Rsop { get; set; }

        public string GpoId { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("State")]
        public string TargetState { get; set; }

        public string CurrentState { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("ModuleNames")]
        public ModuleNames ModuleNames { get; set; } = new ModuleNames();

        public bool IsPresent { get; set; }

        public override bool Equals(object obj)
        {
            if (CurrentState != null && GpoId != null)
            {
                var otherPolicy = obj as Policy;

                if (otherPolicy == null)
                {
                    return false;
                }

                if (ModuleNames.ValueElementData != null)
                {
                    return CurrentState == otherPolicy.CurrentState && ModuleNames.ValueElementData == otherPolicy.ModuleNames.ValueElementData;
                }

                return TargetState.Equals(otherPolicy.CurrentState);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 17;
        }
    }
}
