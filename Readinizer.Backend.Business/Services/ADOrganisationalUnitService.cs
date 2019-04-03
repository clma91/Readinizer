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
                DirectoryEntry startingPoint = new DirectoryEntry("LDAP://" + domain.Name);
                DirectorySearcher searcher = new DirectorySearcher(startingPoint, "(objectCategory=organizationalUnit)");

                

                foreach (SearchResult searchResult in searcher.FindAll())
                {
                    ADOrganisationalUnit foundOU = new ADOrganisationalUnit();
                    foundOU.Name = searchResult.Properties["ou"][0].ToString();
                    foundOU.LdapPath = searchResult.Path.ToString();
                    foundOU.DomainRefId = domain.Id;

                    adOrganisationalUnitsRepository.Add(foundOU);
                }
            }
            await adOrganisationalUnitsRepository.SaveChangesAsync();
        }
    }
}
