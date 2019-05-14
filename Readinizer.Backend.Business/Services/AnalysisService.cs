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
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.DataAccess.UnityOfWork;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Business.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalysisService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Analyse()
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

                var fileName = xml.Name.Replace(".xml", "");
                var organisationalUnitRefId = 0;
                var siteRefId = 0;
                int.TryParse(fileName.Substring(0, fileName.IndexOf("-")).Replace("Ou_", ""), out organisationalUnitRefId);
                int.TryParse(fileName.Substring(fileName.IndexOf("-") + 1).Replace("Site_", ""), out siteRefId);

                var allRsopGpos = GetAllRsopGpos(rsopJson);
                var auditSettings = AnalyseAuditSettings(rsopJson);
                var securityOptions = AnalyseSecurityOptions(rsopJson);
                var policies = AnalysePolicies(rsopJson);
                var registrySettings = AnalyseRegistrySetting(rsopJson);

                var organisationalUnit = unitOfWork.OrganisationalUnitRepository.GetByID(organisationalUnitRefId);
                var rsop = new Rsop
                {
                    Domain = organisationalUnit.ADDomain,
                    OrganisationalUnit = organisationalUnit,
                    Site = unitOfWork.SiteRepository.GetByID(siteRefId),
                    AuditSettings = auditSettings.OrderBy(x => x.SubcategoryName).ToList(),
                    Policies = policies.OrderBy(x => x.Name).ToList(),
                    RegistrySettings = registrySettings.OrderBy(x => x.Name).ToList(),
                    SecurityOptions = securityOptions.OrderBy(x => x.Description).ToList(),
                    Gpos = allRsopGpos
                };

                rsops.Add(rsop);
            }
            unitOfWork.RSoPRepository.AddRange(rsops);
            await unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// This method is used to convert the RSoP from XML to JSON for easier handling.
        /// But more important, the namespaces (q1:, q2:, ..., qn:) of the XML are removed with the Regex-Pattern "q[0-9]:".
        /// 
        /// --------- Before: ---------
        /// <q2:AuditSetting>
        ///     ...
        ///     <q2:SubcategoryName>Audit File System</q2:SubcategoryName>
        ///     <q2:SettingValue>0</q2:SettingValue>
        /// </q2:AuditSetting>
        ///
        /// --------- After: ---------
        /// <AuditSetting>
        ///     ...
        ///     <SubcategoryName>Audit File System</SubcategoryName>
        ///     <SettingValue>0</SettingValue>
        /// </AuditSetting>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static JObject XmlToJson(XmlNode doc)
        {
            var jsonText = JsonConvert.SerializeXmlNode(doc);
            var namespaceRegex = new Regex("q[0-9]:");
            var jsonNonNamespaceText = namespaceRegex.Replace(jsonText, "");
            var rsop = JObject.Parse(jsonNonNamespaceText);
            return rsop;
        }

        private static List<AuditSetting> AnalyseAuditSettings(JToken rsop)
        {
            var recommendedAuditSettings = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedAuditSettings"], new List<AuditSetting>());

            var jsonAuditSettings = rsop.SelectToken("$..AuditSetting");
            var auditSettings = new List<AuditSettingJson>();
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
                x.GpoId = auditSettings.Where(y => y.SubcategoryName.Equals(x.SubcategoryName))
                    .Select(z => z.Gpo.GpoIdentifier.Id)
                    .DefaultIfEmpty("NoGpoId")
                    .FirstOrDefault();
                return x;
            }).ToList();
        }


        private static List<SecurityOption> AnalyseSecurityOptions(JToken rsop)
        {
            var recommendedSecurityOptions = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedSecurityOptions"], new List<SecurityOption>());

            var jsonSecurityOptions = rsop.SelectToken("$..SecurityOptions");
            var securityOptions = new List<SecurityOptionJson>();
            GetSettings(jsonSecurityOptions, securityOptions);

            var presentSecurityOptions = recommendedSecurityOptions.Join(securityOptions,
                recommendedSecurityOption => recommendedSecurityOption.KeyName,
                securityOption => securityOption.KeyName,
                (recommendedSecurityOption, x) => recommendedSecurityOption).ToList();

            return recommendedSecurityOptions.Select(x =>
            {
                x.IsPresent = presentSecurityOptions.Contains(x);
                x.CurrentSettingNumber = securityOptions.Where(y => y.CurrentSettingNumber == x.TargetSettingNumber)
                    .Select(z => z.CurrentSettingNumber)
                    .DefaultIfEmpty("NotDefined")
                    .FirstOrDefault();

                x.CurrentDisplay.DisplayBoolean = securityOptions.Where(y => y.CurrentDisplay != null && y.CurrentDisplay.DisplayBoolean != null &&
                                                                             y.KeyName.Equals(x.KeyName))
                    .Select(z => z.CurrentDisplay.DisplayBoolean)
                    .DefaultIfEmpty("NotDefined")
                    .FirstOrDefault();
                x.CurrentDisplay.Name = x.TargetDisplay.Name;

                x.GpoId = securityOptions.Where(y => y.CurrentSettingNumber.Equals(x.CurrentSettingNumber))
                .Select(z => z.Gpo.GpoIdentifier.Id)
                .DefaultIfEmpty("NoGpoId")
                .FirstOrDefault();

                return x;
            }).ToList();
        }

        private static List<RegistrySetting> AnalyseRegistrySetting(JToken rsop)
        {
            var recommendedRegistrySettings = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedRegistrySettings"], new List<RegistrySetting>());

            var jsonRegistrySettings = rsop.SelectToken("$..RegistrySetting");
            var registrySettings = new List<RegistrySettingJson>();
            GetSettings(jsonRegistrySettings, registrySettings);

            var presentRegistrySettings = recommendedRegistrySettings.Join(registrySettings,
                recommendedRegistrySetting => recommendedRegistrySetting.KeyPath,
                registrySetting => registrySetting.KeyPath,
                (recommendedRegistrySetting, x) => recommendedRegistrySetting).ToList();

            return recommendedRegistrySettings.Select(w =>
            {
                w.IsPresent = presentRegistrySettings.Contains(w);
                w.CurrentValue = registrySettings.Where(x => x.CurrentValue != null)
                    .Where(y => y.CurrentValue.Name.Equals(w.TargetValue.Name))
                    .Select(z => z.CurrentValue)
                    .DefaultIfEmpty(new Value())
                    .FirstOrDefault();
                w.GpoId = registrySettings.Where(x => x.CurrentValue != null)
                    .Where(y => y.CurrentValue.Name.Equals(w.TargetValue.Name))
                    .Select(z => z.Gpo.GpoIdentifier.Id)
                    .DefaultIfEmpty("NoGpoId")
                    .FirstOrDefault();
                return w;
            }).ToList();
        }

        private static List<Policy> AnalysePolicies(JToken rsop)
        {
            var recommendedPolicies = GetRecommendedSettings(ConfigurationManager.AppSettings["RecommendedPolicySettings"], new List<Policy>());

            var jsonPolicies = rsop.SelectToken("$..Policy");
            var policies = new List<PolicyJson>();
            GetSettings(jsonPolicies, policies);

            var presentPolicies = recommendedPolicies.Join(policies,
                recommendedPolicy => recommendedPolicy.Name,
                policy => policy.Name,
                (recommendedPolicy, x) => recommendedPolicy).ToList();

            return recommendedPolicies.Select(x =>
            {
                x.IsPresent = presentPolicies.Contains(x);
                x.CurrentState = policies.Where(y => y.Name.Equals(x.Name))
                    .Select(z => z.CurrentState)
                    .DefaultIfEmpty("Disabled")
                    .FirstOrDefault();
                // TODO: Further Process -> Get Module Names
                x.GpoId = policies.Where(y => y.Name.Equals(x.Name))
                    .Select(z => z.Gpo.GpoIdentifier.Id)
                    .DefaultIfEmpty("NoGpoId")
                    .FirstOrDefault();
                return x;
            }).ToList();
        }
        
        private static List<T> GetRecommendedSettings<T>(string path, List<T> recommendedSettings)
        {
            recommendedSettings = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));
            return recommendedSettings;
        }

        private static void GetSettings<T>(JToken jsonSettings, List<T> settings) where T : new()
        {
            if (!(jsonSettings is null))
            {
                if (jsonSettings.Type is JTokenType.Array)
                {
                    var jsonSettingsList = jsonSettings.Children().ToList();
                    foreach (var jsonSetting in jsonSettingsList)
                    {
                        try
                        {
                            var setting = jsonSetting.ToObject<T>();
                            settings.Add(setting);
                        }
                        catch
                        {
                            var setting = new T();
                            settings.Add(setting);
                        }
                    }
                }
                else
                {
                    try
                    {
                        var setting = jsonSettings.ToObject<T>();
                        settings.Add(setting);
                    }
                    catch
                    {
                        var setting = new T();
                        settings.Add(setting);
                    }
                }
            }
        }

        private static List<Gpo> GetAllRsopGpos(JObject rsop)
        {
            var jsonGpos = rsop["Rsop"]["ComputerResults"]["GPO"];
            var gpos = new List<Gpo>();

            var serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            try
            {
                gpos = JsonConvert.DeserializeObject<List<Gpo>>(jsonGpos.ToString(), serializerSettings);
            }
            catch
            {
                var setting = new Gpo();
                var undefinedGpo = setting.NotIdentified();
                gpos.Add(undefinedGpo);
            }

            return gpos;
        }
    }
}
