using System.Data.Entity.ModelConfiguration.Conventions;
using System.Runtime.CompilerServices;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.ModelsJson;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

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
        public virtual DbSet<OrganisationalUnit> OrganisationalUnits { get; set; }
        public virtual DbSet<Computer> Computers { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<Rsop> RSoPs { get; set; }
        public virtual DbSet<RsopPot> RSoPPots { get; set; }

        public virtual DbSet<AuditSetting> AuditSettings { get; set; }
        public virtual DbSet<Policy> Policies { get; set; }
        public virtual DbSet<RegistrySetting> RegistrySettings { get; set; }
        public virtual DbSet<SecurityOption> SecurityOptions { get; set; }
        public virtual DbSet<Gpo> Gpos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<ADDomain>().ToTable(nameof(ADDomain));
            modelBuilder.Entity<ADDomain>().HasKey(x => x.ADDomainId);
            modelBuilder.Entity<ADDomain>().HasMany(x => x.SubADDomains).WithOptional().HasForeignKey(x => x.ParentId);
            modelBuilder.Entity<ADDomain>().Property(x => x.Name).IsRequired();


            modelBuilder.Entity<Site>().ToTable(nameof(Site));
            modelBuilder.Entity<Site>().HasKey(x => x.SiteId);
            modelBuilder.Entity<Site>().Property(x => x.Name).IsRequired();


            modelBuilder.Entity<Site>().HasMany<ADDomain>(x => x.Domains).WithMany(x => x.Sites).Map(x =>
            {
                x.MapLeftKey("SiteRefId");
                x.MapRightKey("ADDomainRefId");
                x.ToTable("SiteADDomain");
            });


            modelBuilder.Entity<OrganisationalUnit>().ToTable(nameof(OrganisationalUnit));
            modelBuilder.Entity<OrganisationalUnit>().HasKey(x => x.OrganisationalUnitId);
            modelBuilder.Entity<OrganisationalUnit>().HasMany(x => x.SubOrganisationalUnits).WithOptional().HasForeignKey(x => x.ParentId);
            modelBuilder.Entity<OrganisationalUnit>().HasRequired(x => x.ADDomain).WithMany(x => x.OrganisationalUnits).HasForeignKey(x => new { x.ADDomainRefId });


            modelBuilder.Entity<Computer>().ToTable(nameof(Computer));
            modelBuilder.Entity<Computer>().HasKey(x => x.ComputerId);
            modelBuilder.Entity<Computer>().HasRequired(x => x.Site).WithMany(x => x.Computers).HasForeignKey(x => new {x.SiteRefId});
           

           modelBuilder.Entity<OrganisationalUnit>().HasMany<Computer>(x => x.Computers).WithMany(x => x.OrganisationalUnits).Map(x =>
            {
                x.MapLeftKey("OrganisationalUnitRefId");
                x.MapRightKey("ComputerRefId");
                x.ToTable("OrganisationalUnitComputer");
            });


            modelBuilder.Entity<RsopPot>().ToTable(nameof(RsopPot));
            modelBuilder.Entity<RsopPot>().HasKey(x => x.RsopPotId);
            modelBuilder.Entity<RsopPot>().HasMany(x => x.Rsops).WithOptional().HasForeignKey(x => x.RsopPotRefId);
            modelBuilder.Entity<RsopPot>().HasOptional(x => x.Domain).WithMany(x => x.RsopPots);


            modelBuilder.Entity<Rsop>().ToTable(nameof(Rsop));
            modelBuilder.Entity<Rsop>().HasKey(x => x.RsopId);
            modelBuilder.Entity<Rsop>().HasOptional(x => x.Domain).WithMany(x => x.Rsops);
            modelBuilder.Entity<Rsop>().HasOptional(x => x.OrganisationalUnit).WithMany(x => x.Rsops);
            modelBuilder.Entity<Rsop>().HasOptional(x => x.Site).WithMany(x => x.Rsops);


            modelBuilder.Entity<AuditSetting>().ToTable(nameof(AuditSetting));
            modelBuilder.Entity<AuditSetting>().HasKey(x => x.AuditSettingId);
            modelBuilder.Entity<AuditSetting>().HasRequired(x => x.Rsop).WithMany(x => x.AuditSettings)
                .HasForeignKey(x => new { x.RsopRefId} );


            modelBuilder.Entity<Policy>().ToTable(nameof(Policy));
            modelBuilder.Entity<Policy>().HasKey(x => x.PolicyId);
            modelBuilder.Entity<Policy>().HasRequired(x => x.Rsop).WithMany(x => x.Policies)
                .HasForeignKey(x => new { x.RsopRefId });


            modelBuilder.Entity<RegistrySetting>().ToTable(nameof(RegistrySetting));
            modelBuilder.Entity<RegistrySetting>().HasKey(x => x.RegistrySettingId);
            modelBuilder.Entity<RegistrySetting>().HasRequired(x => x.Rsop).WithMany(x => x.RegistrySettings)
                .HasForeignKey(x => new { x.RsopRefId });


            modelBuilder.Entity<SecurityOption>().ToTable(nameof(SecurityOption));
            modelBuilder.Entity<SecurityOption>().HasKey(x => x.SecurityOptionId);
            modelBuilder.Entity<SecurityOption>().HasRequired(x => x.Rsop).WithMany(x => x.SecurityOptions)
                .HasForeignKey(x => new { x.RsopRefId });


            modelBuilder.Entity<Gpo>().ToTable(nameof(Gpo));
            modelBuilder.Entity<Gpo>().HasKey(x => x.GpoId);
            modelBuilder.Entity<Gpo>().HasRequired(x => x.Rsop).WithMany(x => x.Gpos)
                .HasForeignKey(x => new { x.RsopRefId });


            modelBuilder.ComplexType<Path>();
            modelBuilder.ComplexType<Value>();
            modelBuilder.ComplexType<Identifier>();
            modelBuilder.ComplexType<Link>();
            modelBuilder.ComplexType<ListBox>();
            modelBuilder.ComplexType<ModuleNames>();
            modelBuilder.ComplexType<Display>();
        }
    }
}