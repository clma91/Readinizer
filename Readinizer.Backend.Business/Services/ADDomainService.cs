using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using AD = System.DirectoryServices.ActiveDirectory;
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

            foreach (AD.Domain domain in Forest.GetCurrentForest().Domains)
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
            var currentDomain = new ADDomain
            {
                Name = AD.Domain.GetCurrentDomain().Name,
                SubADDomain = new List<ADDomain>()
            };
            var childDomains = GetChildDomains(AD.Domain.GetCurrentDomain());
            
            foreach (var childDomain in childDomains)
            {
                currentDomain.SubADDomain.Add(childDomain);
                adDomainRepository.Add(childDomain);
            }
            adDomainRepository.Add(currentDomain);

            return adDomainRepository.SaveChangesAsync();
        }

        private List<ADDomain> GetChildDomains(AD.Domain currentDomain)
        {
            var childDomains = new List<ADDomain>();
            
            if (currentDomain.Children == null)
            {
                return null;
            } else
            {
                foreach(AD.Domain childDomain in currentDomain.Children)
                {
                    childDomains.Add(new ADDomain()
                    {
                        Name = childDomain.Name
                    });
                }
            }

            return childDomains;
        }

        public bool isDomainInForest(string fullyQualifiedDomainName)
        {
            bool isInForest = false;

            try
            {
                foreach (AD.Domain domain in Forest.GetCurrentForest().Domains)
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
