using Newtonsoft.Json;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class SecurityOption
    {
        [JsonIgnore]
        public int SecurityOptionId { get; set; }

        [JsonIgnore]
        public int RsopRefId { get; set; }

        [JsonIgnore]
        public Rsop Rsop { get; set; }

        public string GpoId { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Path")]
        public string Path { get; set; }

        [JsonProperty("KeyName")]
        public string KeyName { get; set; }

        [JsonProperty("SettingNumber")]
        public string TargetSettingNumber { get; set; }

        [JsonProperty("TargetDisplay")]
        public Display TargetDisplay { get; set; }

        public Display CurrentDisplay { get; set; } = new Display();

        public string CurrentSettingNumber { get; set; }

        public bool IsPresent { get; set; }

        public bool IsStatusOk => CurrentDisplay.DisplayBoolean.Equals(TargetDisplay.DisplayBoolean);

        public override bool Equals(object obj)
        {
            if (CurrentDisplay.Name != null && CurrentDisplay.DisplayBoolean != null)
            {
                var securityOption = obj as SecurityOption;

                if (securityOption == null)
                {
                    return false;
                }

                return CurrentDisplay.Name == securityOption.CurrentDisplay.Name &&
                       CurrentDisplay.DisplayBoolean == securityOption.CurrentDisplay.DisplayBoolean;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Description.GetHashCode() * 17;
        }
    }
}
