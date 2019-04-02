using Readinizer.Backend.DataAccess;
using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.DataAccess.Repositories
{
    public class ADOrganisationalUnitRepository
    {
        private readonly ReadinizerDbContext context;

        public ADOrganisationalUnitRepository() { }

        public ADOrganisationalUnitRepository(ReadinizerDbContext context)
        {
            this.context = context;
        }

        public void Add(ADOrganisationalUnit organisationalUnit)
        {
            context.ADOrganisationalUnits.Add(organisationalUnit);
        }

    }
}
