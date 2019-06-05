using System.Linq;
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
