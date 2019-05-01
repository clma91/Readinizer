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
    public class RSoPService : IRSoPService
    {
        private readonly IUnitOfWork unitOfWork;

        public RSoPService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task getRSoPOfReachableComputers()
        {
            List<OrganisationalUnit> allOUs = await unitOfWork.OrganisationalUnitRepository.GetAllEntities();
            List<int> collectedSiteIds = new List<int>();
            foreach (OrganisationalUnit OU in allOUs)
            {
                collectedSiteIds.Clear();

                string domainName = unitOfWork.ADDomainRepository.GetByID(OU.ADDomainRefId).Name;

                    foreach (var computer in OU.Computers)
                    {

                        if (!collectedSiteIds.Contains(computer.SiteRefId) && PingHost(computer.IpAddress))
                        {
                            computer.PingSuccessfull = true;
                            unitOfWork.ComputerRepository.Update(computer);

                            OU.HasReachableComputer = true;
                            unitOfWork.OrganisationalUnitRepository.Update(OU);

                            collectedSiteIds.Add(computer.SiteRefId);

                            getRSoP(computer.ComputerName + "." + domainName,
                                computer.ComputerName,
                                System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                        }
                    }
                
                await unitOfWork.SaveChangesAsync();
            }
        }



        public void getRSoP(string computerpath, string computername, string user)
        {
            try
            {
                GPRsop test = new GPRsop(RsopMode.Logging, "");
                test.LoggingMode = LoggingMode.Computer;
                test.LoggingComputer = computerpath;
                test.LoggingUser = user;
                test.CreateQueryResults();
                test.GenerateReportToFile(ReportType.Xml, (AppDomain.CurrentDomain.BaseDirectory + "\\RSOP\\"+computername+".xml"));

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
                PingReply reply = pinger.Send(ipAddress, 200); //TODO set ping timeout
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