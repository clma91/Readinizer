using System.Linq;
using Readinizer.Backend.DataAccess.Context;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Repositories
{
    public class OrganisationalUnitRepository : GenericRepository<OrganizationalUnit>
    {
        public OrganisationalUnitRepository(ReadinizerDbContext context) : base(context)
        {
        }

        public virtual OrganizationalUnit GetOrganisationalUnitByNames(string ouName, string domainName)
        {
            return context.Set<OrganizationalUnit>().FirstOrDefault(x => x.Name == ouName && x.ADDomain.Name == domainName);
        }
    }
}
