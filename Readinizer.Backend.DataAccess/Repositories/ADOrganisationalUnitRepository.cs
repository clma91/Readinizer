using Readinizer.Backend.DataAccess;
using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Repositories
{
    public class ADOrganisationalUnitRepository
    {
        public IEnumerable<ADOrganisationalUnit> loadADOrganisationalUnits()
        {
            using (var db = new ReadinizerDbContext())
            {
                
            }
            return null;
        }

        public bool saveADOrganisationalUnits(IEnumerable<ADOrganisationalUnit> organisationalUnits)
        {
            return false;
        }
    }
}
