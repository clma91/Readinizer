﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Services
{
    public class RSoPPotService : IRSoPPotService
    {
        private readonly IUnitOfWork unitOfWork;
        private static int index;

        public RSoPPotService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task GenerateRsopPots()
        {
            index = 1;

            var rsops = await unitOfWork.RsopRepository.GetAllEntities();
            var sortedRsopsByDomain = rsops.OrderBy(x => x.Domain.ParentId).ToList();
            var rsopPots = FillRsopPotList(sortedRsopsByDomain);

            unitOfWork.RsopPotRepository.AddRange(rsopPots);

            await unitOfWork.SaveChangesAsync();
        }

        public List<RsopPot> FillRsopPotList(List<Rsop> sortedRsopsByDomain)
        {
            var rsopPots = new List<RsopPot>();
            AddRsopPot(sortedRsopsByDomain.First());

            foreach (var rsop in sortedRsopsByDomain.Skip(1))
            {
                var foundPot = RsopPotsEqual(rsopPots, rsop);

                if (foundPot == null)
                {
                    AddRsopPot(rsop);
                }
            }

            void AddRsopPot(Rsop rsop)
            {
                rsopPots.Add(RsopPotFactory(rsop));
            }

            return rsopPots;
        }

        private static RsopPot RsopPotFactory(Rsop rsop)
        {
            return new RsopPot
            {
                Name = index++.ToString() + ". Group of identical security settings",
                DateTime = DateTime.Now.ToString("g", CultureInfo.InvariantCulture),
                Domain = rsop.Domain,
                Rsops = new List<Rsop> { rsop }
            };
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
                else if (!rsopPots.Any(x => RsopAndRsopPotsOuEqual(rsop, x.Rsops.First())))
                {
                    foundPot = RsopPotFactory(rsop);
                    rsopPots.Add(foundPot);
                    unitOfWork.RsopPotRepository.Add(foundPot);
                }
            }

            await unitOfWork.SaveChangesAsync();
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

                if (RsopAndRsopPotsOuEqual(rsop, currentRsop)) continue;

                pot.Rsops.Add(rsop);
                foundPot = pot;
                break;
            }

            return foundPot;
        }

        private static bool RsopAndRsopPotsOuEqual(Rsop rsop, Rsop currentRsop)
        {
            var organisationalUnitsEqual = currentRsop.OrganisationalUnit.Name.Equals(rsop.OrganisationalUnit.Name);
            if (organisationalUnitsEqual) return true;
            return false;
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
