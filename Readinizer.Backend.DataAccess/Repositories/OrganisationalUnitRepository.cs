using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Repositories
{
    public class OrganisationalUnitRepository : GenericRepository<OrganisationalUnit>
    {
        public OrganisationalUnitRepository(ReadinizerDbContext context) : base(context)
        {
        }

        public virtual OrganisationalUnit GetOrganisationalUnitByNames(string ouName, string domainName)
        {
            return context.Set<OrganisationalUnit>().FirstOrDefault(x => x.Name == ouName && x.ADDomain.Name == domainName);
        }
    }
}
