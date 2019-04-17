using System.Data.Entity;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IReadinizerDbContext
    {
        DbSet<Domain.Models.ADDomain> ADDomains { get; set; }
        DbSet<OrganisationalUnit> ADOrganisationalUnits { get; set; }
        DbSet<Computer> ADOuMembers { get; set; }
        DbSet<Site> ADSites { get; set; }
    }
}