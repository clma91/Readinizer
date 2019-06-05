using System;
using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.DataAccess.Repositories;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson;

namespace Readinizer.Backend.DataAccess.UnityOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ReadinizerDbContext context = new ReadinizerDbContext();
        private GenericRepository<ADDomain> adDomainRepository;
        private GenericRepository<OrganisationalUnit> organisationalUnitRepository;
        private OrganisationalUnitRepository specificOrganisationalUnitRepository;
        private GenericRepository<Computer> computerRepository;
        private GenericRepository<Site> siteRepository;
        private SiteRepository specificSiteRepository;
        private GenericRepository<Rsop> rsopRepository;
        private GenericRepository<RsopPot> rsopPotRepository;
        private GenericRepository<Gpo> gpoRepository;

        public GenericRepository<ADDomain> ADDomainRepository
        {
            get
            {
                if (adDomainRepository == null)
                {
                    adDomainRepository = new GenericRepository<ADDomain>(context);
                }

                return adDomainRepository;
            }
        }

        public GenericRepository<OrganisationalUnit> OrganisationalUnitRepository
        {
            get
            {
                if (organisationalUnitRepository == null)
                {
                    organisationalUnitRepository = new GenericRepository<OrganisationalUnit>(context);
                }

                return organisationalUnitRepository;
            }
        }

        public OrganisationalUnitRepository SpecificOrganisationalUnitRepository
        {
            get
            {
                if (specificOrganisationalUnitRepository == null)
                {
                    specificOrganisationalUnitRepository = new OrganisationalUnitRepository(context);
                }

                return specificOrganisationalUnitRepository;
            }
        }

        public GenericRepository<Computer> ComputerRepository
        {
            get
            {
                if (computerRepository == null)
                {
                    computerRepository = new GenericRepository<Computer>(context);
                }

                return computerRepository;
            }
        }

        public GenericRepository<Site> SiteRepository
        {
            get
            {
                if (siteRepository == null)
                {
                    siteRepository = new GenericRepository<Site>(context);
                }

                return siteRepository;
            }
        }

        public SiteRepository SpecificSiteRepository
        {
            get
            {
                if (specificSiteRepository == null)
                {
                    specificSiteRepository = new SiteRepository(context);
                }

                return specificSiteRepository;
            }
        }

        public GenericRepository<Rsop> RsopRepository
        {
            get
            {
                if (rsopRepository == null)
                {
                    rsopRepository = new GenericRepository<Rsop>(context);
                }

                return rsopRepository;
            }
        }

        public GenericRepository<RsopPot> RsopPotRepository
        {
            get
            {
                if (rsopPotRepository == null)
                {
                    rsopPotRepository = new GenericRepository<RsopPot>(context);
                }

                return rsopPotRepository;
            }
        }

        public GenericRepository<Gpo> GpoRepository
        {
            get
            {
                if (gpoRepository == null)
                {
                    gpoRepository = new GenericRepository<Gpo>(context);
                }

                return gpoRepository;
            }
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        private bool disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
