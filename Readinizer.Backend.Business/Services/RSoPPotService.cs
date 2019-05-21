using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
        private static int index = 1;

        public RSoPPotService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task GenerateRsopPots()
        {
            var rsops = await unitOfWork.RsopRepository.GetAllEntities();
            var rsopPots = new List<RsopPot>();

            AddRsopPot(rsops.First());

            foreach (var rsop in rsops.Skip(1))
            {
                var foundPot = RsopPotsEqual(rsopPots, rsop);

                if (foundPot == null)
                {
                    AddRsopPot(rsop);
                }
            }

            void AddRsopPot(Rsop rsop)
            {
                rsopPots.Add(new RsopPot
                {
                    Name = index++.ToString() + ". Group of identical security settings",
                    DateTime = DateTime.Now.ToString("g", CultureInfo.InvariantCulture),
                    Domain = rsop.Domain,
                    Rsops = new List<Rsop> { rsop }
                });
            }

            unitOfWork.RsopPotRepository.AddRange(rsopPots);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateRsopPots(List<Rsop> rsops)
        {
            var rsopPots = await unitOfWork.RsopPotRepository.GetAllEntities();

            foreach (var rsop in rsops)
            {
                var foundPot = RsopPotsEqual(rsopPots, rsop);

                if (foundPot != null)
                {
                    foundPot.DateTime = DateTime.Now.ToString("g", CultureInfo.InvariantCulture);
                    unitOfWork.RsopPotRepository.Update(foundPot);
                }
            }
        }

        private static RsopPot RsopPotsEqual(List<RsopPot> rsopPots, Rsop rsop)
        {
            RsopPot foundPot = null;

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

                pot.Rsops.Add(rsop);
                foundPot = pot;
                break;
            }

            return foundPot;
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
    }
}
