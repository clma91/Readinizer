using System;
using System.CodeDom;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Microsoft.GroupPolicy;


namespace Readinizer.Backend.Business.Services
{
    public class ADRSoPService : IADRSoPService
    {
        private readonly IADOuMemberRepository adOuMemberRepository;

        public ADRSoPService(IADOuMemberRepository adOuMemberRepository)
        {
            this.adOuMemberRepository = adOuMemberRepository;
        }


        public async Task getRSoPOfOUMembers()
        {

            List<ADOuMember> allMembers = await adOuMemberRepository.getAllOUMembers();

        }

        

        public void getRSoP(string computer, string user)
        {
            try
            {
                GPRsop test = new GPRsop(RsopMode.Logging, "");
                test.LoggingMode = LoggingMode.Computer;
                test.LoggingComputer = computer;
                test.LoggingUser = user;
                test.CreateQueryResults();
                test.GenerateReportToFile(ReportType.Xml, @"C:\Users\lkellenb\Desktop\test.xml");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

    }
}