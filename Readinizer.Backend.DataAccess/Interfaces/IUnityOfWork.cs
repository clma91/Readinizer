using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Repositories;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IUnityOfWork
    {
        GenericRepository<Domain.Models.ADDomain> ADDomainRepository { get; }
        GenericRepository<OrganisationalUnit> ADOrganisationalRepository { get; }
        GenericRepository<Computer> ADOuMemberRepository { get; }
        GenericRepository<Site> ADSiteRepository { get; }
        Task SaveChangesAsync();
        void Dispose(bool disposing);
        void Dispose();
    }
}
