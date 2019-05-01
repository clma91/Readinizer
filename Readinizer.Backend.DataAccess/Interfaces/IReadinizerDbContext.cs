using System.Data.Entity;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IReadinizerDbContext
    {
        DbSet<ADDomain> ADDomains { get; set; }
        DbSet<OrganisationalUnit> OrganisationalUnits { get; set; }
        DbSet<Computer> Computers { get; set; }
        DbSet<Site> Sites { get; set; }
        DbSet<Rsop> Rsops { get; set; }
        DbSet<AuditSettingReco> AuditSettings { get; set; }
        DbSet<PolicyReco> Policies { get; set; }
        DbSet<RegistrySettingReco> RegistrySettings { get; set; }
        DbSet<SecurityOptionReco> SecurityOptions { get; set; }
    }
}