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
        Task<List<ADDomain>> SearchAllDomains();

        bool IsDomainInForest(string fullyQualifiedDomainName);
    }
}
