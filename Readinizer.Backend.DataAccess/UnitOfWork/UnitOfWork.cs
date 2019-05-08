using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.DataAccess.Repositories;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.UnityOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ReadinizerDbContext context = new ReadinizerDbContext();
        private GenericRepository<ADDomain> adDomainRepository;
        private GenericRepository<OrganisationalUnit> organisationalUnitRepository;
        private GenericRepository<Computer> computerRepository;
        private GenericRepository<Site> siteRepository;
        private GenericRepository<Rsop> rSoPRepository;
        private GenericRepository<RsopPot> rSoPPotRepository;

        public GenericRepository<ADDomain> ADDomainRepository
        {
            get
            {
                if (this.adDomainRepository == null)
                {
                    this.adDomainRepository = new GenericRepository<ADDomain>(context);
                }

                return adDomainRepository;
            }
        }

        public GenericRepository<OrganisationalUnit> OrganisationalUnitRepository
        {
            get
            {
                if (this.organisationalUnitRepository == null)
                {
                    this.organisationalUnitRepository = new GenericRepository<OrganisationalUnit>(context);
                }

                return organisationalUnitRepository;
            }
        }

        public GenericRepository<Computer> ComputerRepository
        {
            get
            {
                if (this.computerRepository == null)
                {
                    this.computerRepository = new GenericRepository<Computer>(context);
                }

                return computerRepository;
            }
        }

        public GenericRepository<Site> SiteRepository
        {
            get
            {
                if (this.siteRepository == null)
                {
                    this.siteRepository = new GenericRepository<Site>(context);
                }

                return siteRepository;
            }
        }

        public GenericRepository<Rsop> RSoPRepository
        {
            get
            {
                if (this.rSoPRepository == null)
                {
                    this.rSoPRepository = new GenericRepository<Rsop>(context);
                }

                return rSoPRepository;
            }
        }

        public GenericRepository<RsopPot> RSoPPotRepository
        {
            get
            {
                if (this.rSoPPotRepository == null)
                {
                    this.rSoPPotRepository = new GenericRepository<RsopPot>(context);
                }

                return rSoPPotRepository;
            }
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
