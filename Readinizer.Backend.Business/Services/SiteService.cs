﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using AD = System.DirectoryServices.ActiveDirectory;

namespace Readinizer.Backend.Business.Services
{
    public class SiteService : ISiteService
    {
        private readonly IUnityOfWork unityOfWork;

        public SiteService(IUnityOfWork unityOfWork)
        {
            this.unityOfWork = unityOfWork;
        }

        public async Task SearchAllSites()
        {
            var sites = new List<AD.ActiveDirectorySite>();
            var allDomains = await unityOfWork.ADDomainRepository.GetAllEntities();

            try
            {
                var forestSites = AD.Forest.GetCurrentForest().Sites;

                foreach (AD.ActiveDirectorySite site in forestSites)
                {
                    sites.Add(site);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var models = MapToDomainModel(sites, allDomains);
            unityOfWork.ADSiteRepository.AddRange(models);

            await unityOfWork.SaveChangesAsync();
        }

        private List<Site> MapToDomainModel(List<AD.ActiveDirectorySite> sites, List<Domain.Models.ADDomain> allDomains)
        {
            var adSites = new List<Site>();

            foreach (var site in sites)
            {
                var siteADDomains = new List<ADDomain>();

                var siteDomains = site.Domains;
                foreach (AD.Domain siteDomain in siteDomains)
                {
                    siteADDomains.Add(allDomains.Find(x => x.Name.Equals(siteDomain.Name)));
                }

                var subnets = new List<string>();
                foreach (AD.ActiveDirectorySubnet activeDirectorySubnet in site.Subnets)
                {
                    subnets.Add(activeDirectorySubnet.Name);
                }

                var adSite = new Site { Name = site.Name, Subnets = subnets, Domains = siteADDomains};
                adSites.Add(adSite);
            }

            return adSites;
        }
    }
}
