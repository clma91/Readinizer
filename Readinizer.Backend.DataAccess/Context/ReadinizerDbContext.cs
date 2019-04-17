using Readinizer.Backend.DataAccess.Interfaces;

namespace Readinizer.Backend.DataAccess
{
    using Readinizer.Backend.Domain.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ReadinizerDbContext : DbContext, IReadinizerDbContext
    {
        public ReadinizerDbContext()
            : base("name=ReadinizerDbContext")
        {
        }

        public virtual DbSet<ADDomain> ADDomains { get; set; }
        public virtual DbSet<OrganisationalUnit> ADOrganisationalUnits { get; set; }
        public virtual DbSet<Computer> ADOuMembers { get; set; }
        public virtual DbSet<Site> ADSites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ADDomain>().ToTable(nameof(ADDomain));
            modelBuilder.Entity<ADDomain>().HasKey(x => x.ADDomainId);
            modelBuilder.Entity<ADDomain>().HasMany(x => x.ADSubDomains).WithOptional().HasForeignKey(x => x.ParentId);
            modelBuilder.Entity<ADDomain>().Property(x => x.Name).IsRequired();


            modelBuilder.Entity<Site>().ToTable(nameof(Site));
            modelBuilder.Entity<Site>().HasKey(x => x.SiteId);
            modelBuilder.Entity<Site>().Property(x => x.Name).IsRequired();


            modelBuilder.Entity<Site>().HasMany<ADDomain>(x => x.Domains).WithMany(x => x.Sites).Map(x =>
            {
                x.MapLeftKey("ADSiteRefId");
                x.MapRightKey("ADDomainRefId");
                x.ToTable("ADSiteDomain");
            });


            modelBuilder.Entity<OrganisationalUnit>().ToTable(nameof(OrganisationalUnit));
            modelBuilder.Entity<OrganisationalUnit>().HasKey(x => x.OrganisationalUnitId);
            modelBuilder.Entity<OrganisationalUnit>().HasMany(x => x.SubOrganisationalUnits).WithOptional().HasForeignKey(x => x.ParentId);
            modelBuilder.Entity<OrganisationalUnit>().HasRequired(x => x.ADDomain).WithMany(x => x.ADOrganisationalUnits).HasForeignKey(x => new { x.ADDomainRefId });


            modelBuilder.Entity<Computer>().ToTable(nameof(Computer));
            modelBuilder.Entity<Computer>().HasKey(x => x.ComputerId);


            modelBuilder.Entity<OrganisationalUnit>().HasMany<Computer>(x => x.Computers).WithMany(x => x.OrganisationalUnits).Map(x =>
            {
                x.MapLeftKey("OrganisationalUnitRefId");
                x.MapRightKey("ComputerRefId");
                x.ToTable("OrganisationalUnitComputer");
            });
        }
    }
}