using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Threading.Tasks;
using NetTools;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Services
{
    public class ComputerService : IComputerService
    {
        private readonly IUnitOfWork unitOfWork;


        public ComputerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task GetComputers()
        {
            List<OrganisationalUnit> allOUs = await unitOfWork.OrganisationalUnitRepository.GetAllEntities();
            List<ADDomain> allDomains = await unitOfWork.ADDomainRepository.GetAllEntities();
            List<Site> sites = await unitOfWork.SiteRepository.GetAllEntities();

            List<string> DCnames = getDcNames();

            foreach (OrganisationalUnit OU in allOUs)
            {
                DirectoryEntry entry = new DirectoryEntry(OU.LdapPath);
                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.Filter = ("(objectClass=computer)");
                searcher.SearchScope = SearchScope.OneLevel;

                foreach (SearchResult searchResult in searcher.FindAll())
                {
                    Computer foundMember = new Computer{ OrganisationalUnits = new List<OrganisationalUnit>() };
                    foundMember.ComputerName = searchResult.GetDirectoryEntry().Name.Remove(0, "CN=".Length);
                    foundMember.IsDomainController = DCnames.Contains(foundMember.ComputerName);
                    foundMember.IpAddress = getIP(foundMember, OU, allDomains);
                    foundMember.OrganisationalUnits.Add(OU);
                    foundMember.SiteRefId = getSite(foundMember, sites);

                    unitOfWork.ComputerRepository.Add(foundMember);
                }
            }
            await unitOfWork.SaveChangesAsync();
        }

        List<string> getDcNames()
        {
            List<string> DCs = new List<string>();
            
            foreach (System.DirectoryServices.ActiveDirectory.Domain domain in Forest.GetCurrentForest().Domains)
            {
                try
                {
                    foreach (DomainController dc in domain.DomainControllers)
                    {
                        string dcname = dc.Name.Remove((dc.Name.Length - (dc.Domain.Name.Length + 1)),
                            dc.Domain.Name.Length + 1);
                        DCs.Add(dcname);
                    }
                }
                catch (Exception)
                {
                    return DCs;
                }

            }

            return DCs;
        }

        string getIP(Computer foundMember, OrganisationalUnit OU, List<Domain.Models.ADDomain> allDomains)
        {
            foreach (ADDomain domain in allDomains)
            {
                if (domain.ADDomainId.Equals(OU.ADDomainRefId))
                {
                    foreach (IPAddress address in Dns.GetHostEntry(foundMember.ComputerName + "." + domain.Name)
                        .AddressList)
                    {
                        if (!address.IsIPv6LinkLocal)
                        {
                            return foundMember.IpAddress = address.ToString();
                        }
                    }
                }
            }

            return null;
        }

        int getSite(Computer foundMember, List<Site> sites)
        {
            foreach (Site site in sites)
            {
                foreach (string subnet in site.Subnets)
                {
                    if (IsInRange(foundMember.IpAddress, subnet))
                    {
                        return site.SiteId;
                    }
                }
            }
            return 0;
        }

        public bool IsInRange(string address, string subnet)
        {
            var range = IPAddressRange.Parse(subnet);

            return range.Contains(IPAddress.Parse(address));

        }

    }
}
