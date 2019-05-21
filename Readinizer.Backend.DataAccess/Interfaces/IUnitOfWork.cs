using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Repositories;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        GenericRepository<Domain.Models.ADDomain> ADDomainRepository { get; }
        GenericRepository<OrganisationalUnit> OrganisationalUnitRepository { get; }
        OrganisationalUnitRepository SpecificOrganisationalUnitRepository { get; }
        GenericRepository<Computer> ComputerRepository { get; }
        GenericRepository<Site> SiteRepository { get; }
        SiteRepository SpecificSiteRepository { get; }
        GenericRepository<Rsop> RsopRepository { get; }
        GenericRepository<RsopPot> RsopPotRepository { get; }
        GenericRepository<Gpo> GpoRepository { get; }
        Task SaveChangesAsync();
        void Dispose(bool disposing);
        void Dispose();
    }
}
