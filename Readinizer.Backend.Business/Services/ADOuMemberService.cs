using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
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

        public ADOuMemberService(IADOuMemberRepository adOuMemberRepository, IADOrganisationalUnitRepository adOrganisationalUnitRepository)
        {
            this.adOuMemberRepository = adOuMemberRepository;
            this.adOrganisationalUnitsRepository = adOrganisationalUnitRepository;
        }

        public async Task GetMembersOfOu()
        {
            List<ADOrganisationalUnit> allOUs = await adOrganisationalUnitsRepository.getAllOUs();

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

                    adOuMemberRepository.Add(foundMember);
                }
            }
            await adOuMemberRepository.SaveChangesAsync();
        }
    }
}
