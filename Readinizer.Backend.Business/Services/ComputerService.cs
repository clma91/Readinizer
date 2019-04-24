using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Services
{
    public class ComputerService : IComputerService
    {
        private readonly IUnityOfWork unityOfWork;

        public ComputerService(IUnityOfWork unityOfWork)
        {
            this.unityOfWork = unityOfWork;
        }

        public async Task GetComputers()
        {
            List<OrganisationalUnit> allOUs = await unityOfWork.OrganisationalUnitRepository.GetAllEntities();
            List<ADDomain> allDomains = await unityOfWork.ADDomainRepository.GetAllEntities();
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

                    unityOfWork.ComputerRepository.Add(foundMember);
                }
            }
            await unityOfWork.SaveChangesAsync();
        }

        List<string> getDcNames()
        {
            List<string> DCs = new List<string>();
            foreach (System.DirectoryServices.ActiveDirectory.Domain domain in Forest.GetCurrentForest().Domains)
            {
                foreach (DomainController dc in domain.DomainControllers)
                {
                    string dcname = dc.Name.Remove((dc.Name.Length - (dc.Domain.Name.Length + 1)), dc.Domain.Name.Length + 1);
                    DCs.Add(dcname);
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

    }
}
