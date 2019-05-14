﻿using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Repositories;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        GenericRepository<Domain.Models.ADDomain> ADDomainRepository { get; }
        GenericRepository<OrganisationalUnit> OrganisationalUnitRepository { get; }
        GenericRepository<Computer> ComputerRepository { get; }
        GenericRepository<Site> SiteRepository { get; }
        GenericRepository<Rsop> RSoPRepository { get; }
        GenericRepository<RsopPot> RSoPPotRepository { get; }
        Task SaveChangesAsync();
        void Dispose(bool disposing);
        void Dispose();
    }
}
