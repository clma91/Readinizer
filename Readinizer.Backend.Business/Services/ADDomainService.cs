using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.DirectoryServices.ActiveDirectory;
using AD = System.DirectoryServices.ActiveDirectory;

namespace Readinizer.Backend.Business.Services
{
    public class ADDomainService : IADDomainService
    {

        private readonly IADDomainRepository adDomainRepository;

        public ADDomainService(IADDomainRepository adDomainRepository)
        {
            this.adDomainRepository = adDomainRepository;
        }
        
        public Task SearchAllDomainsFrom(string domainName)
        {
            var searchedDomains = new List<AD.Domain>();
            var currentADDomain = new ADDomain();

            if (isForestRoot(domainName))
            {
                var forestDomains = Forest.GetCurrentForest().Domains;
                searchedDomains.Add(Forest.GetCurrentForest().RootDomain);
                foreach (AD.Domain forestDomain in forestDomains)
                {
                    if (!forestDomain.Name.EndsWith(domainName))
                    {
                        searchedDomains.Add(forestDomain);
                    }
                }
            }
            else
            {
                try
                {
                    searchedDomains.Add(AD.Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, domainName)));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return SearchAllDomains(searchedDomains);
        }


        public Task SearchAllDomains(List<AD.Domain> searchedDomains)
        {
            foreach (var searchedDomain in searchedDomains)
            {
                var currentADDomain = new ADDomain
                {
                    Name = searchedDomain.Name,
                    SubADDomain = new List<ADDomain>(),
                    IsForestRoot = isForestRoot(searchedDomain.Name),
                };
                var childDomains = GetChildDomains(searchedDomain);

                foreach (var childDomain in childDomains)
                {
                    currentADDomain.SubADDomain.Add(childDomain);
                    adDomainRepository.Add(childDomain);
                }
                adDomainRepository.Add(currentADDomain);
            }

            return adDomainRepository.SaveChangesAsync();
        }

        private List<ADDomain> GetChildDomains(AD.Domain currentDomain)
        {
            var childDomains = new List<ADDomain>();

            foreach (AD.Domain childDomain in currentDomain.Children)
            {
                childDomains.Add(new ADDomain()
                {
                    Name = childDomain.Name
                });
                GetChildDomains(childDomain);
            }

            return childDomains;
        }

        private bool isForestRoot(string domainName)
        {
            return Forest.GetCurrentForest().Name.Equals(domainName);
        }

        public bool isDomainInForest(string fullyQualifiedDomainName)
        {
            bool isInForest = false;

            foreach (AD.Domain domain in Forest.GetCurrentForest().Domains)
            {
                if (domain.Name.Equals(fullyQualifiedDomainName))
                {
                    isInForest = true;
                }
            }
            
            return isInForest;
        }
    }
}
