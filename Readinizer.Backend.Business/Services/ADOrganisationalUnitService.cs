using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace Readinizer.Backend.Business.Services
{
    public class ADOrganisationalUnitService
    {
        List<ADOrganisationalUnit> organisationalUnits { get; set; }

        public ADOrganisationalUnitService()
        {
            organisationalUnits = new List<ADOrganisationalUnit>();
        }

        public List<ADOrganisationalUnit> GetAllOrganisationalUnits()
        {
            DirectoryEntry startingPoint = new DirectoryEntry("LDAP://DC=readinizer,DC=ch");
            DirectorySearcher searcher = new DirectorySearcher(startingPoint, "(objectCategory=organizationalUnit)");

            foreach (SearchResult res in searcher.FindAll())
            {
                organisationalUnits.Add(new ADOrganisationalUnit(res.Properties["ou"][0].ToString()));
            }
            return organisationalUnits;
        }
    }
}
