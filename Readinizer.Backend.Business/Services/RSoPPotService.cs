using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Business.Services
{
    public class RSoPPotService : IRSoPPotService
    {
        private readonly IUnitOfWork unitOfWork;

        public RSoPPotService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task GenerateRsopPots()
        {
            var rsops = await unitOfWork.RSoPRepository.GetAllEntities();
            var rsopPots = new List<RsopPot>();
            var i = 1;

            AddRsopPot(rsops.First());

            foreach (var rsop in rsops.Skip(1))
            {
                bool found = false;

                foreach (var pot in rsopPots)
                {
                    var currentRsop = pot.Rsops.FirstOrDefault();
                    if (currentRsop == null) continue;

                    var auditSettingsEqual = SettingsEqual(currentRsop.AuditSettings, rsop.AuditSettings);
                    if (!auditSettingsEqual) continue;

                    var policiesEqual = SettingsEqual(currentRsop.Policies, rsop.Policies);
                    if (!policiesEqual) continue;

                    var registrySettingsEqual = SettingsEqual(currentRsop.RegistrySettings, rsop.RegistrySettings);
                    if (!registrySettingsEqual) continue;

                    var securityOptionsEqual = SettingsEqual(currentRsop.SecurityOptions, rsop.SecurityOptions);
                    if (!securityOptionsEqual) continue;

                    var domainsEqual = currentRsop.Domain.Equals(rsop.Domain);
                    if (!domainsEqual) continue;

                    pot.Rsops.Add(currentRsop);
                    found = true;
                    break;
                }

                if (!found)
                {
                    AddRsopPot(rsop);
                }

            }

            void AddRsopPot(Rsop rsop)
            {
                rsopPots.Add(new RsopPot
                {
                    Name = GetRandomRsopName(),
                    Rsops = new List<Rsop> { rsop }
                });
            }

            unitOfWork.RSoPPotRepository.AddRange(rsopPots);

            await unitOfWork.SaveChangesAsync();
        }

        private static bool SettingsEqual<T>(ICollection<T> currentSettings, ICollection<T> otherSettings)
        {
            if (currentSettings == null || otherSettings == null)
            {
                return (currentSettings == null && otherSettings == null);
            }

            if (currentSettings.Count() != otherSettings.Count())
            {
                return false;
            }

            for (var i = 0; i < currentSettings.Count(); i++)
            {
                if (!currentSettings.ElementAt(i).Equals(otherSettings.ElementAt(i)))
                {
                    return false;
                }
            }

            return true;
        }
        
        private static string GetRandomRsopName()
        {
            var animalNames = JsonConvert.DeserializeObject<List<RandomName>>(File.ReadAllText(ConfigurationManager.AppSettings["AnimalNames"]));
            var adjectives = JsonConvert.DeserializeObject<List<RandomName>>(File.ReadAllText(ConfigurationManager.AppSettings["Adjectives"]));

            var randomKeyAdjectives = new Random(DateTime.Now.Millisecond).Next(0, adjectives.Count);
            var randomKeyAnimals = new Random(DateTime.Now.Millisecond).Next(0, animalNames.Count);
            var adjective = adjectives.ElementAt(randomKeyAdjectives);
            var animalName = animalNames.ElementAt(randomKeyAnimals);
            var rsopName = adjective.Value + " " + animalName.Value;

            return rsopName;
        }
    }
}
