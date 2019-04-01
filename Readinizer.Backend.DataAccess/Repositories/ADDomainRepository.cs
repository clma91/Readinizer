using Readinizer.Backend.DataAccess;
using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Readinizer.Backend.DataAccess.Interfaces;

namespace Readinizer.Backend.DataAccess.Repositories
{
    public class ADDomainRepository : IADDomainRepository
    {
        private readonly ReadinizerDbContext context;

        public ADDomainRepository() { }

        public ADDomainRepository(ReadinizerDbContext context)
        {
            this.context = context;
        }

        public void Add(ADDomain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException(nameof(domain));
            }

            context.ADDomains.Add(domain);
        }

        public Task<ADDomain> GetByDomainName(string domainName)
        {
            return context.ADDomains
                .Where(d => d.Name.Equals(domainName))
                .FirstOrDefaultAsync();
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
