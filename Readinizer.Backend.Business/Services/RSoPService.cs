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

            foreach (OrganisationalUnit OU in allOUs)
            {
                string domainName = unitOfWork.ADDomainRepository.GetByID(OU.ADDomainRefId).Name;
                if (OU.Computers != null)
                {
                    Computer reachableComputer = GetReachableComputer(OU);
                    

                    if (reachableComputer != null)
                    {
                        unitOfWork.ComputerRepository.Update(reachableComputer);

                        OU.HasReachableComputer = true;
                        unitOfWork.OrganisationalUnitRepository.Update(OU);

                        getRSoP(reachableComputer.ComputerName + "." + domainName,
                            reachableComputer.ComputerName,
                            System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    }
                }

                await unitOfWork.SaveChangesAsync();
            }



        }

        private static Computer GetReachableComputer(OrganisationalUnit OU)
        {
            foreach (Computer computer in OU.Computers)
            {
                
                    if (PingHost(computer.IpAddress))
                    {
                        computer.PingSuccessfull = true;

                        return computer;
                    }
                
            }

            return null;
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