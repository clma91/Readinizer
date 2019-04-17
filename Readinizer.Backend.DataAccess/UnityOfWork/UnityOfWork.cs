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
        private GenericRepository<ADDomain> adDomainRepository;
        private GenericRepository<ADOrganisationalUnit> adOrganisationalUnitRepository;
        private GenericRepository<ADOuMember> adOuMemberRepository;
        private GenericRepository<ADSite> adSiteRepository;

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

        public GenericRepository<ADOrganisationalUnit> ADOrganisationalRepository
        {
            get
            {
                if (this.adOrganisationalUnitRepository == null)
                {
                    this.adOrganisationalUnitRepository = new GenericRepository<ADOrganisationalUnit>(context);
                }

                return adOrganisationalUnitRepository;
            }
        }

        public GenericRepository<ADOuMember> ADOuMemberRepository
        {
            get
            {
                if (this.adOuMemberRepository == null)
                {
                    this.adOuMemberRepository = new GenericRepository<ADOuMember>(context);
                }

                return adOuMemberRepository;
            }
        }

        public GenericRepository<ADSite> ADSiteRepository
        {
            get
            {
                if (this.adSiteRepository == null)
                {
                    this.adSiteRepository = new GenericRepository<ADSite>(context);
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
