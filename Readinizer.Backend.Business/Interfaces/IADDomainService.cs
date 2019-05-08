using Readinizer.Backend.Domain.Models;
using AD = System.DirectoryServices.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Business.Interfaces
{
    public interface IADDomainService
    {
        Task SearchAllDomains(string domainname);

        bool IsDomainInForest(string fullyQualifiedDomainName);

        Task AddThisDomain(string domainname);
    }
}
