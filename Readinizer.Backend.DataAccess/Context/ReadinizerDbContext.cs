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

        public virtual DbSet<ADDomain> ADDomains { get; set; }
        public virtual DbSet<ADOrganisationalUnit> ADOrganisationalUnits { get; set; }
    }
}