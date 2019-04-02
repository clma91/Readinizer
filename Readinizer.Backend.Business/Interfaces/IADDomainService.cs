using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Business.Interfaces
{
    public interface IADDomainService
    {
        Task SearchAllDomains();

        ADDomain SearchDomain(string fullyQualifiedDomainName);

        bool isDomainInForest(string fullyQualifiedDomainName);
    }
}
