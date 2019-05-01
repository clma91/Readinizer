﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson;

namespace Readinizer.Backend.Business.Services
{
    public class AnalysisService : IAnalysisService
    {
        public AnalysisService() { }

        public void Analyse()
        {
            var receivedRsopPath = ConfigurationManager.AppSettings["ReceivedRSoP"];
            var directoryInfo = new DirectoryInfo(receivedRsopPath);
            var rsopXml = directoryInfo.GetFiles("*.xml");
            var rsops = new List<Rsop>();

            foreach (var xml in rsopXml)
            {
                var doc = new XmlDocument();
                doc.Load(xml.FullName);
                var rsopJson = XmlToJson(doc);

                var policies = AnalysePolicies(rsopJson);
                var auditSettings = AnalyseAuditSettings(rsopJson);
                var registrySettings = AnalyseRegistrySetting(rsopJson);
                var securityOptions = AnalyseSecurityOptions(rsopJson);
                var allRsopGpos = GetAllRsopGpos(rsopJson);
                var rsop = new Rsop
                {
                    AuditSettings = auditSettings,
                    Policies = policies,
                    RegistrySettings = registrySettings,
                    SecurityOptions = securityOptions,
                    Gpos = allRsopGpos
                };
                rsops.Add(rsop);
            }
        }

        private static JObject XmlToJson(XmlDocument doc)
        {
            var jsonText = JsonConvert.SerializeXmlNode(doc);
            var namespaceRegex = new Regex("q[0-9]:");
            var jsonNonNamespaceText = namespaceRegex.Replace(jsonText, "");
            var rsop = JObject.Parse(jsonNonNamespaceText);
            return rsop;
        }

        private static List<Gpo> GetAllRsopGpos(JObject rsop)
        {
            var jsonGpos = rsop["Rsop"]["ComputerResults"]["GPO"];
            var gpos = new List<Gpo>();
            GetSettings(jsonGpos, gpos);
            return gpos;
        }

        private static List<PolicyReco> AnalysePolicies(JToken rsop)
        {
            var recommendedPolicies = new List<PolicyReco>();
            recommendedPolicies = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedPolicySettings"], recommendedPolicies);

            var jsonPolicies = rsop.SelectToken("$..Policy");
            var policies = new List<Policy>();
            GetSettings(jsonPolicies, policies);

            var presentPolicy = recommendedPolicies.Join(policies,
                recommendedPolicy => recommendedPolicy.Name,
                policy => policy.Name,
                (policy, x) => policy).ToList();

            return recommendedPolicies.Select(x =>
            {
                x.IsPresent = presentPolicy.Contains(x);
                x.CurrentState = policies.Where(y => y.CurrentState.Equals(x.CurrentState))
                    .Select(z => z.CurrentState)
                    .DefaultIfEmpty("Disabled")
                    .FirstOrDefault();
                return x;
            }).ToList();
        }

        private static List<SecurityOptionReco> AnalyseSecurityOptions(JToken rsop)
        {
            var recommendedSecurityOptions = new List<SecurityOptionReco>();
            recommendedSecurityOptions = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedSecurityOptions"], recommendedSecurityOptions);

            var jsonSecurityOptions = rsop.SelectToken("$..SecurityOptions");
            var securityOptions = new List<SecurityOption>();
            GetSettings(jsonSecurityOptions, securityOptions);

            var presentSecurityOptions = recommendedSecurityOptions.Join(securityOptions,
                recommendedSecurityOption => recommendedSecurityOption.KeyName,
                securityOption => securityOption.KeyName,
                (securityOption, x) => securityOption).ToList();

            return recommendedSecurityOptions.Select(x =>
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
        }

        private static List<RegistrySettingReco> AnalyseRegistrySetting(JToken rsop)
        {
            var recommendedRegistrySettings = new List<RegistrySettingReco>();
            recommendedRegistrySettings = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedRegistrySettings"], recommendedRegistrySettings);

            var jsonRegistrySettings = rsop.SelectToken("$..RegistrySetting");
            var registrySettings = new List<RegistrySetting>();
            GetSettings(jsonRegistrySettings, registrySettings);

            var presentRegistrySettings = recommendedRegistrySettings.Join(registrySettings,
                recommendedRegistrySetting => recommendedRegistrySetting.KeyPath,
                registrySetting => registrySetting.KeyPath,
                (recommendedRegistrySetting, x) => recommendedRegistrySetting).ToList();

            return recommendedRegistrySettings.Select(w =>
            {
                w.IsPresent = presentRegistrySettings.Exists(x => x.Name.Equals(w.Name));
                w.CurrentValue = registrySettings.Where(x => x.CurrentValue != null)
                    .Where(y => y.CurrentValue.Name.Equals(w.TargetValue.Name))
                    .Select(z => z.CurrentValue)
                    .DefaultIfEmpty(null)
                    .FirstOrDefault();
                return w;
            }).ToList();
        }

        private static List<AuditSettingReco> AnalyseAuditSettings(JToken rsop)
        {
            var recommendedAuditSettings = new List<AuditSettingReco>();
            recommendedAuditSettings = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedAuditSettings"], recommendedAuditSettings);
            
            var jsonAuditSettings = rsop.SelectToken("$..AuditSetting");
            var auditSettings = new List<AuditSetting>();
            GetSettings(jsonAuditSettings, auditSettings);

            var presentAuditSettings = recommendedAuditSettings.Join(auditSettings,
                recommendedAuditSetting => recommendedAuditSetting.SubcategoryName,
                auditSetting => auditSetting.SubcategoryName,
                (recommendedAuditSetting, x) => recommendedAuditSetting).ToList();

            return recommendedAuditSettings.Select(x =>
            {
                x.IsPresent = presentAuditSettings.Contains(x);
                x.CurrentSettingValue = auditSettings.Where(y => y.SubcategoryName.Equals(x.SubcategoryName))
                    .Select(z => z.CurrentSettingValue)
                    .DefaultIfEmpty(AuditSettingValue.NoAuditing)
                    .FirstOrDefault();
                return x;
            }).ToList();
        }

        private static List<T> GetRecommendedSettings<T>(string path, List<T> recommendedSettings)
        {
            recommendedSettings = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));
            return recommendedSettings;
        }

        private static void GetSettings<T>(JToken jsonSettings, List<T> settings)
        {
            // TODO: What happens if jsonSetting is Null?
            if (!(jsonSettings is null))
            {
                if (jsonSettings.Type is JTokenType.Array)
                {
                    var jsonSettingsList = jsonSettings.Children().ToList();
                    foreach (var jsonSetting in jsonSettingsList)
                    {
                        T setting = jsonSetting.ToObject<T>();
                        settings.Add(setting);
                    }
                }
                else
                {
                    T setting = jsonSettings.ToObject<T>();
                    settings.Add(setting);
                }
            }
        }
    }
}
