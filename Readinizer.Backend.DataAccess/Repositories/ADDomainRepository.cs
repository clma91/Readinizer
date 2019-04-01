using Readinizer.Backend.DataAccess;
using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Repositories
{
    public class ADDomainRepository
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

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
