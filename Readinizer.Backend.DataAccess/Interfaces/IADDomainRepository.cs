using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IADDomainRepository : IDisposable
    {
        void Add(ADDomain domain);

        void AddRange(List<ADDomain> domains);

        Task<ADDomain> GetByDomainName(string domainName);

        Task SaveChangesAsync();

        Task<List<ADDomain>> GetAllDomains();
    }
}
