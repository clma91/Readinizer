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
    public class ADOuMemberService : IADOuMemberService
    {
        private readonly IADOuMemberRepository adOuMemberRepository;
        private readonly IADOrganisationalUnitRepository adOrganisationalUnitsRepository;
        private readonly IADDomainRepository adDomainRepository;

        public ADOuMemberService(IADOuMemberRepository adOuMemberRepository, IADOrganisationalUnitRepository adOrganisationalUnitRepository, IADDomainRepository adDomainRepository)
        {
            this.adOuMemberRepository = adOuMemberRepository;
            this.adOrganisationalUnitsRepository = adOrganisationalUnitRepository;
            this.adDomainRepository = adDomainRepository;
        }

        public async Task GetMembersOfOu()
        {
            List<ADOrganisationalUnit> allOUs = await adOrganisationalUnitsRepository.getAllOUs();
            List<ADDomain> allDomains = await adDomainRepository.GetAllDomains();
            List<string> DCnames = getDcNames();

            foreach (ADOrganisationalUnit OU in allOUs)
            {
                DirectoryEntry entry = new DirectoryEntry(OU.LdapPath);
                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.Filter = ("(objectClass=computer)");
                searcher.SearchScope = SearchScope.OneLevel;

                foreach (SearchResult searchResult in searcher.FindAll())
                {
                    ADOuMember foundMember = new ADOuMember();
                    foundMember.ComputerName = searchResult.GetDirectoryEntry().Name.Remove(0, "CN=".Length);
                    foundMember.OURefId = OU.ADOrganisationalUnitId;
                    foundMember.IsDomainController = DCnames.Contains(foundMember.ComputerName);
                    foundMember.IpAddress = getIP(foundMember, OU, allDomains);

                    adOuMemberRepository.Add(foundMember);
                }
            }
            await adOuMemberRepository.SaveChangesAsync();
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

        string getIP(ADOuMember foundMember, ADOrganisationalUnit OU, List<ADDomain> allDomains)
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
