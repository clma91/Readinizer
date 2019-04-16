using System.Data.Entity;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IReadinizerDbContext
    {
        DbSet<ADDomain> ADDomains { get; set; }
        DbSet<ADOrganisationalUnit> ADOrganisationalUnits { get; set; }
        DbSet<ADOuMember> ADOuMembers { get; set; }
        DbSet<ADSite> ADSites { get; set; }
    }
}