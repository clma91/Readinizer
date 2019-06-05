using Newtonsoft.Json;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class RegistrySetting
    {
        [JsonIgnore]
        public int RegistrySettingId { get; set; }

        [JsonIgnore]
        public int RsopRefId { get; set; }

        [JsonIgnore]
        public Rsop Rsop { get; set; }

        public string GpoId { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Path")]
        public string Path { get; set; }

        [JsonProperty("KeyPath")]
        public string KeyPath { get; set; }

        [JsonProperty("TargetValue")]
        public Value TargetValue { get; set; }

        public Value CurrentValue { get; set; } = new Value();

        public bool IsPresent { get; set; }

        public bool IsStatusOk => CurrentValue.Number.Equals(TargetValue.Number);

        public override bool Equals(object obj)
        {
            if (GpoId != null)
            {
                var registrySetting = obj as RegistrySetting;

                if (registrySetting == null)
                {
                    return false;
                }

                return CurrentValue.Name == registrySetting.CurrentValue.Name && CurrentValue.Number == registrySetting.CurrentValue.Number;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 17;
        }
    }
}
