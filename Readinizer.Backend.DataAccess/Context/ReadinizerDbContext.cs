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
        public virtual DbSet<ADOrganisationalUnit> ADOrganisationalUnits { get; set; }
        public virtual DbSet<ADOuMember> ADOuMembers { get; set; }
        public virtual DbSet<ADSite> ADSites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ADDomain>().ToTable(nameof(ADDomain));
            modelBuilder.Entity<ADDomain>().HasKey(x => x.ADDomainId);
            modelBuilder.Entity<ADDomain>().HasMany(x => x.ADSubDomains).WithOptional().HasForeignKey(x => x.ParentId);
            modelBuilder.Entity<ADDomain>().Property(x => x.Name).IsRequired();


            modelBuilder.Entity<ADSite>().ToTable(nameof(ADSite));
            modelBuilder.Entity<ADSite>().HasKey(x => x.ADSiteId);
            modelBuilder.Entity<ADSite>().Property(x => x.Name).IsRequired();


            //modelBuilder.Entity<ADSite>().HasMany<ADDomain>(x => x.Domains).WithMany(x => x.Sites).Map(x =>
            //{
            //    x.MapLeftKey("ADSiteRefId");
            //    x.MapRightKey("ADDomainRefId");
            //    x.ToTable("ADSiteDomain");
            //});


            modelBuilder.Entity<ADOrganisationalUnit>().ToTable(nameof(ADOrganisationalUnit));
            modelBuilder.Entity<ADOrganisationalUnit>().HasKey(x => x.ADOrganisationalUnitId);
            modelBuilder.Entity<ADOrganisationalUnit>().HasMany(x => x.SubADOrganisationalUnits).WithOptional().HasForeignKey(x => x.ParentId);
            modelBuilder.Entity<ADOrganisationalUnit>().HasRequired(x => x.ADDomain).WithMany(x => x.ADOrganisationalUnits).HasForeignKey(x => new { x.ADDomainRefId });


            modelBuilder.Entity<ADOuMember>().ToTable(nameof(ADOuMember));
            modelBuilder.Entity<ADOuMember>().HasKey(x => x.ADOuMemberId);
            modelBuilder.Entity<ADOuMember>().HasRequired(x => x.ADOrganisationalUnit).WithMany(x => x.ADOuMembers).HasForeignKey(x => new { x.OURefId });
        }
    }
}