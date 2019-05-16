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
        DbSet<Rsop> RSoPs { get; set; }
        DbSet<RsopPot> RSoPPots { get; set; }
        DbSet<AuditSetting> AuditSettings { get; set; }
        DbSet<Policy> Policies { get; set; }
        DbSet<RegistrySetting> RegistrySettings { get; set; }
        DbSet<SecurityOption> SecurityOptions { get; set; }
        DbSet<Gpo> Gpos { get; set; }
    }
}