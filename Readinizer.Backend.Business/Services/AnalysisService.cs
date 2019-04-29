using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.Domain.ModelsJson;

namespace Readinizer.Backend.Business.Services
{
    public class AnalysisService : IAnalysisService
    {

        public AnalysisService() { }

        public void Analyse()
        {
            var receivedRsopPath = ConfigurationManager.AppSettings["ReceivedRSoP"];
            DirectoryInfo directoryInfo = new DirectoryInfo(receivedRsopPath);
            FileInfo[] rsopXml = directoryInfo.GetFiles("*.xml");
            
            // TODO: XML structure is not always the same:( -> could be fixed with namespace-removal
            foreach (var xml in rsopXml)
            {
                var doc = new XmlDocument();
                doc.Load(xml.FullName);
                var rsop = XmlToJson(doc);
                var faultyPolicies = AnalysePolicies(rsop);
                var faultyAuditSettings = AnalyseAuditSettings(rsop);
                var faultyRegistrySettings = AnalyseRegistrySetting(rsop);
                var faultySecurityOptions = AnalyseSecurityOptions(rsop);
            }
        }

        private static JObject XmlToJson(XmlDocument doc)
        {
            var jsonText = JsonConvert.SerializeXmlNode(doc);
            var rsop = JObject.Parse(jsonText);
            return rsop;
        }

        private static List<Gpo> GetAllRsopGpos(JObject rsop)
        {
            var jsonGpos = rsop["Rsop"]["ComputerResults"]["GPO"].Children().ToList();
            var gpos = new List<Gpo>();
            GetSettings(jsonGpos, gpos);
            return gpos;
        }

        private static List<PolicyReco> AnalysePolicies(JObject rsop)
        {
            var faultyPolicies = new List<PolicyReco>();
            var recommendedPolicies = new List<PolicyReco>();
            recommendedPolicies = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedPolicySettings"], recommendedPolicies);

            var jsonPolicies = rsop["Rsop"]["ComputerResults"]["ExtensionData"][6]["Extension"]["q7:Policy"].Children().ToList();
            var policies = new List<Policy>();
            GetSettings(jsonPolicies, policies);

            var presentPolicy = recommendedPolicies.Join(policies,
                recommendedPolicy => recommendedPolicy.Name,
                policy => policy.Name,
                (policy, x) => policy).ToList();

            faultyPolicies = recommendedPolicies.Select(x =>
            {
                x.IsPresent = presentPolicy.Contains(x);
                x.CurrentState = policies.Where(y => y.CurrentState.Equals(x.CurrentState))
                    .Select(z => z.CurrentState)
                    .DefaultIfEmpty("Disabled")
                    .FirstOrDefault();
                return x;
            }).ToList();

            return faultyPolicies;
        }

        private static List<SecurityOptionReco> AnalyseSecurityOptions(JObject rsop)
        {
            var faultySecurityOptions = new List<SecurityOptionReco>();
            var recommendedSecurityOptions = new List<SecurityOptionReco>();
            recommendedSecurityOptions = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedSecurityOptions"], recommendedSecurityOptions);

            var jsonSecurityOptions = rsop["Rsop"]["ComputerResults"]["ExtensionData"][2]["Extension"]["q3:SecurityOptions"].Children().ToList();
            var securityOptions = new List<SecurityOption>();
            GetSettings(jsonSecurityOptions, securityOptions);

            var presentSecurityOptions = recommendedSecurityOptions.Join(securityOptions,
                recommendedSecurityOption => recommendedSecurityOption.KeyName,
                securityOption => securityOption.KeyName,
                (securityOption, x) => securityOption).ToList();

            faultySecurityOptions = recommendedSecurityOptions.Select(x =>
            {
                x.IsPresent = presentSecurityOptions.Exists(y => x.KeyName.Equals(x.KeyName));
                x.CurrentSettingNumber = securityOptions.Where(y => y.CurrentSettingNumber.Equals(x.CurrentSettingNumber))
                    .Select(z => z.CurrentSettingNumber)
                    .DefaultIfEmpty("NotDefined")
                    .FirstOrDefault();
                if (x.CurrentDisplay != null)
                {
                    x.CurrentDisplay.DisplayBoolean = securityOptions
                        .Where(y => y.CurrentDisplay.DisplayBoolean.Equals(x.CurrentDisplay.DisplayBoolean))
                        .Select(z => z.CurrentDisplay.DisplayBoolean)
                        .DefaultIfEmpty("NotDefined")
                        .FirstOrDefault();
                }

                return x;
            }).ToList();

            return faultySecurityOptions;
        }

        private static List<RegistrySettingReco> AnalyseRegistrySetting(JObject rsop)
        {
            var faultyRegistrySettings = new List<RegistrySettingReco>();
            var recommendedRegistrySettings = new List<RegistrySettingReco>();
            recommendedRegistrySettings = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedRegistrySettings"], recommendedRegistrySettings);

            var jsonRegistrySettings = rsop["Rsop"]["ComputerResults"]["ExtensionData"][6]["Extension"]["q7:RegistrySetting"].Children().ToList();
            var registrySettings = new List<RegistrySetting>();
            GetSettings(jsonRegistrySettings, registrySettings);

            var presentRegistrySettings = recommendedRegistrySettings.Join(registrySettings,
                recommendedRegistrySetting => recommendedRegistrySetting.KeyPath,
                registrySetting => registrySetting.KeyPath,
                (recommendedRegistrySetting, x) => recommendedRegistrySetting).ToList();

            faultyRegistrySettings = recommendedRegistrySettings.Select(w =>
            {
                w.IsPresent = presentRegistrySettings.Exists(x => x.Name.Equals(w.Name));
                w.CurrentValue = registrySettings.Where(x => x.CurrentValue != null)
                    // TODO: Check Name
                    .Select(z => z.CurrentValue)
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();
                return w;
            }).ToList();

            return faultyRegistrySettings;
        }

        private static List<AuditSettingReco> AnalyseAuditSettings(JObject rsop)
        {
            var faultyAuditSettings = new List<AuditSettingReco>();
            var recommendedAuditSettings = new List<AuditSettingReco>();
            recommendedAuditSettings = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedAuditSettings"], recommendedAuditSettings);

            var jsonAuditSettings = rsop["Rsop"]["ComputerResults"]["ExtensionData"][1]["Extension"]["q2:AuditSetting"].Children().ToList();
            var auditSettings = new List<AuditSetting>();
            GetSettings(jsonAuditSettings, auditSettings);

            var presentAuditSettings = recommendedAuditSettings.Join(auditSettings,
                recommendedAuditSetting => recommendedAuditSetting.SubcategoryName,
                auditSetting => auditSetting.SubcategoryName,
                (recommendedAuditSetting, x) => recommendedAuditSetting).ToList();

            faultyAuditSettings = recommendedAuditSettings.Select(x =>
            {
                x.IsPresent = presentAuditSettings.Contains(x);
                x.CurrentSettingValue = auditSettings.Where(y => y.SubcategoryName.Equals(x.SubcategoryName))
                    .Select(z => z.CurrentSettingValue)
                    .DefaultIfEmpty(AuditSettingValue.NoAuditing)
                    .FirstOrDefault();
                return x;
            }).ToList();

            return faultyAuditSettings;
        }

        private static List<T> GetRecommendedSettings<T>(string path, List<T> recommendedSettings)
        {
            recommendedSettings = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));
            return recommendedSettings;
        }

        private static void GetSettings<T>(List<JToken> jsonSettings, List<T> settings)
        {
            foreach (var jsonSetting in jsonSettings)
            {
                T setting = jsonSetting.ToObject<T>();
                settings.Add(setting);
            }
        }

    }
}
