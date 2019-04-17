using System.Data.Entity;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IReadinizerDbContext
    {
        DbSet<ADDomain> ADDomains { get; set; }
        DbSet<OrganisationalUnit> OrganisationalUnits { get; set; }
        DbSet<Computer> Computers { get; set; }
        DbSet<Site> Sites { get; set; }
    }
}