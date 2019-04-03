namespace Readinizer.Backend.DataAccess
{
    using Readinizer.Backend.Domain.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ReadinizerDbContext : DbContext
    {
        public ReadinizerDbContext()
            : base("name=ReadinizerDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ADDomain>().ToTable(nameof(ADDomain));
            modelBuilder.Entity<ADDomain>().HasKey(x => x.ADDomainId);
            modelBuilder.Entity<ADDomain>().HasMany(x => x.SubADDomain).WithOptional().HasForeignKey(x => x.ParentId);
            modelBuilder.Entity<ADDomain>().Property(x => x.Name).IsRequired();
            

            modelBuilder.Entity<ADOrganisationalUnit>().ToTable(nameof(ADOrganisationalUnit));
            modelBuilder.Entity<ADOrganisationalUnit>().HasKey(x => x.ADOrganisationalUnitId);
            modelBuilder.Entity<ADOrganisationalUnit>().HasMany(x => x.SubADOrganisationalUnits).WithOptional().HasForeignKey(x => x.ParentId);
            modelBuilder.Entity<ADOrganisationalUnit>().HasRequired(x => x.ADDomain).WithMany(x => x.ADOrganisationalUnits).HasForeignKey(x => new { x.ADDomainRefId });


            modelBuilder.Entity<ADOuMember>().ToTable(nameof(ADOuMember));
            modelBuilder.Entity<ADOuMember>().HasKey(x => x.ADOuMemberId);
            modelBuilder.Entity<ADOuMember>().HasRequired(x => x.ADOrganisationalUnit).WithMany(x => x.ADOuMembers).HasForeignKey(x => new { x.OURefId });
        }

        public virtual DbSet<ADDomain> ADDomains { get; set; }
        public virtual DbSet<ADOrganisationalUnit> ADOrganisationalUnits { get; set; }
        public virtual DbSet<ADOuMember> ADOuMembers { get; set; }
    }
}