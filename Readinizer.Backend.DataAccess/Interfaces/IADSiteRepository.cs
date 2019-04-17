using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IADSiteRepository : IDisposable
    {
        void Add(ADSite site);
        void AddRange(List<ADSite> sites);
        Task<List<ADSite>> GetAllSites();
        Task<ADSite> GetBySiteName(string siteName);
        Task SaveChangesAsync();
    }
}