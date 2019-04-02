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

        public ADOrganisationalUnitService(IADOrganisationalUnitRepository adOrganisationalUnitRepository)
        {
            this.adOrganisationalUnitsRepository = adOrganisationalUnitRepository;
        }

        public  Task GetAllOrganisationalUnits(string domainPath)
        {
            DirectoryEntry startingPoint = new DirectoryEntry(domainPath);
            DirectorySearcher searcher = new DirectorySearcher(startingPoint, "(objectCategory=organizationalUnit)");

            foreach (SearchResult res in searcher.FindAll())
            {
                adOrganisationalUnitsRepository.Add(new ADOrganisationalUnit(res.Properties["ou"][0].ToString(), res.Path.ToString()));
            }
            return adOrganisationalUnitsRepository.SaveChangesAsync();
        }
    }
}
