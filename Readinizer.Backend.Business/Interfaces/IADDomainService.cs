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
        Task<List<string>> SearchDomains(string domainName, bool subdomainsChecked);

        bool IsDomainInForest(string fullyQualifiedDomainName);
    }
}
