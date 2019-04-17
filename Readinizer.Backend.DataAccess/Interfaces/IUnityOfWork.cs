using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Repositories;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IUnityOfWork
    {
        GenericRepository<ADDomain> ADDomainRepository { get; }
        GenericRepository<ADOrganisationalUnit> ADOrganisationalRepository { get; }
        GenericRepository<ADOuMember> ADOuMemberRepository { get; }
        GenericRepository<ADSite> ADSiteRepository { get; }
        Task SaveChangesAsync();
        void Dispose(bool disposing);
        void Dispose();
    }
}
