using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;

namespace Readinizer.Backend.Business.Services
{
    public class ADOrganisationalUnitService : IADOrganisationalUnitService
    {
        private readonly IADOrganisationalUnitRepository adOrganisationalUnitsRepository;
        private readonly IADDomainRepository adDomainRepository;

        public ADOrganisationalUnitService(IADOrganisationalUnitRepository adOrganisationalUnitRepository, IADDomainRepository aDDomainRepository)
        {
            this.adOrganisationalUnitsRepository = adOrganisationalUnitRepository;
            this.adDomainRepository = aDDomainRepository;
        }

        public async Task GetAllOrganisationalUnits()
        {
            List<ADDomain> allDomains = await adDomainRepository.GetAllDomains();
            
            foreach (ADDomain domain in allDomains)
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain.Name);
                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.Filter = ("(objectCategory=organizationalUnit)");
                searcher.SearchScope = SearchScope.OneLevel;

                foreach (SearchResult searchResult in searcher.FindAll())
                {
                    ADOrganisationalUnit foundOU = new ADOrganisationalUnit();
                    foundOU.Name = searchResult.Properties["ou"][0].ToString();
                    foundOU.LdapPath = searchResult.Path;
                    foundOU.ADDomainRefId = domain.ADDomainId;
                    foundOU.SubADOrganisationalUnits = GetChildOUs(foundOU.LdapPath, foundOU);

                    adOrganisationalUnitsRepository.Add(foundOU);
                }

                DirectorySearcher defaultContainerSearcher = new DirectorySearcher(entry);
                defaultContainerSearcher.Filter = ("(objectCategory=Container)");
                defaultContainerSearcher.Filter = ("(CN=Computers)"); 
                foreach (SearchResult defaultContainers in defaultContainerSearcher.FindAll())
                {
                    ADOrganisationalUnit foundContainer = new ADOrganisationalUnit();
                    foundContainer.Name = defaultContainers.Properties["cn"][0].ToString();
                    foundContainer.LdapPath = defaultContainers.Path;
                    foundContainer.ADDomainRefId = domain.ADDomainId;

                    adOrganisationalUnitsRepository.Add(foundContainer);
                }

            }
            await adOrganisationalUnitsRepository.SaveChangesAsync();
        }

        public List<ADOrganisationalUnit> GetChildOUs(string ldapPath, ADOrganisationalUnit parentOU)
        {
            List<ADOrganisationalUnit> childOUs = new List<ADOrganisationalUnit>();

            DirectoryEntry childEntry = new DirectoryEntry(ldapPath);
            DirectorySearcher childSearcher = new DirectorySearcher(childEntry);
            childSearcher.Filter = ("(objectCategory=organizationalUnit)");
            childSearcher.SearchScope = SearchScope.OneLevel;

            foreach (SearchResult childResult in childSearcher.FindAll())
            {
                ADOrganisationalUnit childOU = new ADOrganisationalUnit();
                childOU.Name = childResult.Properties["ou"][0].ToString();
                childOU.LdapPath = childResult.Path;
                childOU.ADDomainRefId = parentOU.ADDomainRefId;
                childOU.SubADOrganisationalUnits = GetChildOUs(childOU.LdapPath, childOU);

                childOUs.Add(childOU);

                adOrganisationalUnitsRepository.Add(childOU);
            }

            return childOUs;
        }
    }
}
