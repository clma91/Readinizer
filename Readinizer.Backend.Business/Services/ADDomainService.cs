using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Interfaces;

namespace Readinizer.Backend.Business.Services
{
    public class ADDomainService : IADDomainService
    {

        private readonly IADDomainRepository adDomainRepository;

        public ADDomainService(IADDomainRepository adDomainRepository)
        {
            this.adDomainRepository = adDomainRepository;
        }

        public ADDomain SearchDomain(string fullyQualifiedDomainName)
        {
            ADDomain searchedDomain = new ADDomain();

            foreach (System.DirectoryServices.ActiveDirectory.Domain domain in Forest.GetCurrentForest().Domains)
            {
                if (domain.Name.Equals(fullyQualifiedDomainName))
                {
                    searchedDomain.Name = domain.Name;
                }
                else
                {
                    throw new Exception("This domain does not exist in this forest");
                }
            }

            return searchedDomain;
        }

        public Task SearchAllDomains()
        {
            ADDomain currentDomain = new ADDomain();
            currentDomain.Name = System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain().Name;
            List<System.DirectoryServices.ActiveDirectory.Domain> childDomains = new List<System.DirectoryServices.ActiveDirectory.Domain>();

            childDomains = GetChildDomains(System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain());

            adDomainRepository.Add(currentDomain);
            foreach (System.DirectoryServices.ActiveDirectory.Domain domain in childDomains)
            {
                ADDomain childDomain = new ADDomain();
                childDomain.Name = domain.Name;
                adDomainRepository.Add(childDomain);
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

            try
            {
                foreach (System.DirectoryServices.ActiveDirectory.Domain domain in Forest.GetCurrentForest().Domains)
                {
                    if (domain.Name.Equals(fullyQualifiedDomainName))
                    {
                        isInForest = true;
                    }
                }
            } catch (ActiveDirectoryOperationException e)
            {
                // TODO: add logic to catch exception
            }
            
            return isInForest;
        }
    }
}
