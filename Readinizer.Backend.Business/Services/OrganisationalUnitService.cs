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
    public class OrganisationalUnitService : IOrganisationalUnitService
    {
        private readonly IUnityOfWork unityOfWork;

        public OrganisationalUnitService(IUnityOfWork unityOfWork)
        {
            this.unityOfWork = unityOfWork;
        }

        public async Task GetAllOrganisationalUnits()
        {
            List<Domain.Models.ADDomain> allDomains = await unityOfWork.ADDomainRepository.GetAllEntities();
            
            foreach (Domain.Models.ADDomain domain in allDomains)
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain.Name);
                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.Filter = ("(objectCategory=organizationalUnit)");
                searcher.SearchScope = SearchScope.OneLevel;
                var foundOUs = new List<OrganisationalUnit>();

                foreach (SearchResult searchResult in searcher.FindAll())
                {
                    OrganisationalUnit foundOU = new OrganisationalUnit();
                    foundOU.Name = searchResult.Properties["ou"][0].ToString();
                    foundOU.LdapPath = searchResult.Path;
                    foundOU.ADDomainRefId = domain.ADDomainId;
                    foundOU.SubOrganisationalUnits = GetChildOUs(foundOU.LdapPath, foundOU);

                    foundOUs.Add(foundOU);
                }

                DirectorySearcher defaultContainerSearcher = new DirectorySearcher(entry);
                defaultContainerSearcher.Filter = ("(objectCategory=Container)");
                defaultContainerSearcher.Filter = ("(CN=Computers)"); 
                foreach (SearchResult defaultContainers in defaultContainerSearcher.FindAll())
                {
                    OrganisationalUnit foundContainer = new OrganisationalUnit();
                    foundContainer.Name = defaultContainers.Properties["cn"][0].ToString();
                    foundContainer.LdapPath = defaultContainers.Path;
                    foundContainer.ADDomainRefId = domain.ADDomainId;

                    foundOUs.Add(foundContainer);
                }

                unityOfWork.OrganisationalUnitRepository.AddRange(foundOUs);
            }
            await unityOfWork.SaveChangesAsync();
        }

        public List<OrganisationalUnit> GetChildOUs(string ldapPath, OrganisationalUnit parentOU)
        {
            List<OrganisationalUnit> childOUs = new List<OrganisationalUnit>();

            DirectoryEntry childEntry = new DirectoryEntry(ldapPath);
            DirectorySearcher childSearcher = new DirectorySearcher(childEntry);
            childSearcher.Filter = ("(objectCategory=organizationalUnit)");
            childSearcher.SearchScope = SearchScope.OneLevel;

            foreach (SearchResult childResult in childSearcher.FindAll())
            {
                OrganisationalUnit childOU = new OrganisationalUnit();
                childOU.Name = childResult.Properties["ou"][0].ToString();
                childOU.LdapPath = childResult.Path;
                childOU.ADDomainRefId = parentOU.ADDomainRefId;
                childOU.SubOrganisationalUnits = GetChildOUs(childOU.LdapPath, childOU);

                childOUs.Add(childOU);

                unityOfWork.OrganisationalUnitRepository.Add(childOU);
            }

            return childOUs;
        }
    }
}
