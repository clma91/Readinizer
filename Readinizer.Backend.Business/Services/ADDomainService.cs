using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Business.Services
{
    public class ADDomainService
    {
        ADDomainRepository adDomainRepository = new ADDomainRepository();

        public Task SearchAllDomains()
        {
            System.DirectoryServices.ActiveDirectory.Domain currentDomain = System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain();
            List<System.DirectoryServices.ActiveDirectory.Domain> childDomains = new List<System.DirectoryServices.ActiveDirectory.Domain>();

            childDomains = GetChildDomains(currentDomain);

            adDomainRepository.Add(new ADDomain(currentDomain.Name, false, false));
            foreach (System.DirectoryServices.ActiveDirectory.Domain domain in childDomains)
            {
                adDomainRepository.Add(new ADDomain(domain.Name, false, false));
            }

            return adDomainRepository.SaveChangesAsync();
        }

        private List<System.DirectoryServices.ActiveDirectory.Domain> GetChildDomains(System.DirectoryServices.ActiveDirectory.Domain currentDomain)
        {
            List<System.DirectoryServices.ActiveDirectory.Domain> childDomains = new List<System.DirectoryServices.ActiveDirectory.Domain>();
            
            if (currentDomain.Children == null)
            {
                return null;
            } else
            {
                foreach(System.DirectoryServices.ActiveDirectory.Domain childDomain in currentDomain.Children)
                {
                    childDomains.Add(childDomain);
                }
            }

            return childDomains;
        }

        public bool isDomainInForest(string fullyQualifiedDomainName)
        {
            bool isInForest = false;

            foreach (System.DirectoryServices.ActiveDirectory.Domain domain in Forest.GetCurrentForest().Domains)
            {
                if (domain.Name.Equals(fullyQualifiedDomainName))
                {
                    isInForest = true;
                }
            }

            return isInForest;
        }

        public ADDomain SearchDomain(string fullyQualifiedDomainName)
        {
            ADDomain searchedDomain = new ADDomain("", false, false);

            foreach (System.DirectoryServices.ActiveDirectory.Domain domain in Forest.GetCurrentForest().Domains)
            {
                if (domain.Name.Equals(fullyQualifiedDomainName))
                {
                    searchedDomain = new ADDomain(domain.Name, false, false);
                } else
                {
                    throw new Exception("This domain does not exist in this forest");
                }
            }

            return searchedDomain;
        }

    }
}
