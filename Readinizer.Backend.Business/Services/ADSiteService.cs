using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.DataAccess.Repositories;
using Readinizer.Backend.Domain.Models;
using AD = System.DirectoryServices.ActiveDirectory;

namespace Readinizer.Backend.Business.Services
{
    public class ADSiteService : IADSiteService
    {
        private readonly IADSiteRepository adSiteRepository;
        private readonly IADDomainRepository adDomainRepository;

        public ADSiteService(IADSiteRepository adSiteRepository, IADDomainRepository adDomainRepository)
        {
            this.adSiteRepository = adSiteRepository;
            this.adDomainRepository = adDomainRepository;
        }

        public async Task SearchAllSites()
        {
            var sites = new List<AD.ActiveDirectorySite>();
            var allDomains = await adDomainRepository.GetAllDomains();

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
            //adSiteRepository.AddRange(models);

            await adSiteRepository.SaveChangesAsync();
        }

        private List<ADSite> MapToDomainModel(List<AD.ActiveDirectorySite> sites, List<ADDomain> allDomains)
        {
            var adSites = new List<ADSite>();

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

                var adSite = new ADSite { Name = site.Name, Subnets = subnets, Domains = siteADDomains};
                adSiteRepository.Add(adSite);
                adSites.Add(adSite);
            }

            return adSites;
        }
    }
}
