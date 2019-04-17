using System;
using System.CodeDom;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net.NetworkInformation;
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
        private readonly IADOrganisationalUnitRepository adOrganisationalUnitRepository;

        public ADRSoPService(IADOuMemberRepository adOuMemberRepository, IADOrganisationalUnitRepository adOrganisationalUnitRepository)
        {
            this.adOuMemberRepository = adOuMemberRepository;
            this.adOrganisationalUnitRepository = adOrganisationalUnitRepository;
        }


        public async Task getRSoPOfOUMembers()
        {

            List<ADOuMember> allMembers = await adOuMemberRepository.getAllOUMembers();
            List<ADOrganisationalUnit> allOUs = await adOrganisationalUnitRepository.getAllOUs();

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
                test.GenerateReportToFile(ReportType.Xml, AppDomain.CurrentDomain.BaseDirectory + computer +".xml");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }


        static bool PingHost(string ipAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(ipAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
                return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

    }
}