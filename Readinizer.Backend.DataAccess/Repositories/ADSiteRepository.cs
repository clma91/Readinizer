using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Repositories
{
    public class ADSiteRepository : IADSiteRepository
    {
        private readonly ReadinizerDbContext context;

        public ADSiteRepository(ReadinizerDbContext context)
        {
            this.context = context;
        }

        public void Add(ADSite site)
        {
            if (site == null)
            {
                throw new ArgumentNullException(nameof(site));
            }

            context.ADSites.Add(site);
        }

        public void AddRange(List<ADSite> sites)
        {
            context.ADSites.AddRange(sites);
        }

        public Task<List<ADSite>> GetAllSites()
        {
            return context.ADSites.ToListAsync<ADSite>();
        }

        public Task<ADSite> GetBySiteName(string siteName)
        {
            return context.ADSites
                .Where(d => d.Name.Equals(siteName))
                .FirstOrDefaultAsync();
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
