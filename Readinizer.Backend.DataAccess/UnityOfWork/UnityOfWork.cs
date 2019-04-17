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
    public class UnityOfWork : IDisposable, IUnityOfWork
    {
        private ReadinizerDbContext context = new ReadinizerDbContext();
        private GenericRepository<Domain.Models.ADDomain> adDomainRepository;
        private GenericRepository<OrganisationalUnit> adOrganisationalUnitRepository;
        private GenericRepository<Computer> adOuMemberRepository;
        private GenericRepository<Site> adSiteRepository;

        public GenericRepository<Domain.Models.ADDomain> ADDomainRepository
        {
            get
            {
                if (this.adDomainRepository == null)
                {
                    this.adDomainRepository = new GenericRepository<Domain.Models.ADDomain>(context);
                }

                return adDomainRepository;
            }
        }

        public GenericRepository<OrganisationalUnit> ADOrganisationalRepository
        {
            get
            {
                if (this.adOrganisationalUnitRepository == null)
                {
                    this.adOrganisationalUnitRepository = new GenericRepository<OrganisationalUnit>(context);
                }

                return adOrganisationalUnitRepository;
            }
        }

        public GenericRepository<Computer> ADOuMemberRepository
        {
            get
            {
                if (this.adOuMemberRepository == null)
                {
                    this.adOuMemberRepository = new GenericRepository<Computer>(context);
                }

                return adOuMemberRepository;
            }
        }

        public GenericRepository<Site> ADSiteRepository
        {
            get
            {
                if (this.adSiteRepository == null)
                {
                    this.adSiteRepository = new GenericRepository<Site>(context);
                }

                return adSiteRepository;
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
