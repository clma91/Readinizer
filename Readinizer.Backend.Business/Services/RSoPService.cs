using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Microsoft.GroupPolicy;
using System.IO;

namespace Readinizer.Backend.Business.Services
{
    public class RSoPService : IRSoPService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISysmonService sysmonService;
        private readonly IPingService pingService;

        public RSoPService(IUnitOfWork unitOfWork, ISysmonService sysmonService, IPingService pingService)
        {
            this.unitOfWork = unitOfWork;
            this.sysmonService = sysmonService;
            this.pingService = pingService;
        }


        public async Task getRSoPOfReachableComputers()
        {
            clearOldRsops();
            List<OrganisationalUnit> allOUs = await unitOfWork.OrganisationalUnitRepository.GetAllEntities();
            List<ADDomain> allDomains = await unitOfWork.ADDomainRepository.GetAllEntities();
            List<int> collectedSiteIds = new List<int>();
            foreach (OrganisationalUnit OU in allOUs)
            {
                collectedSiteIds.Clear();

                var domain = allDomains.Find(x => x.ADDomainId == OU.ADDomainRefId);

                if(OU.Computers != null)
                {
                    foreach (var computer in OU.Computers)
                    {
                        if (!collectedSiteIds.Contains(computer.SiteRefId) && pingService.isPingable(computer.IpAddress))
                        {
                            computer.PingSuccessful = true;
                            unitOfWork.ComputerRepository.Update(computer);

                            OU.HasReachableComputer = true;
                            unitOfWork.OrganisationalUnitRepository.Update(OU);

                            collectedSiteIds.Add(computer.SiteRefId);

                            getRSoP(computer.ComputerName + "." + domain.Name,
                                OU.OrganisationalUnitId, computer.SiteRefId,
                                System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                        }

                    }
                }
                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task getRSoPOfReachableComputersAndCheckSysmon(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                serviceName = "Sysmon";
            }

            clearOldRsops();
            List<OrganisationalUnit> allOUs = await unitOfWork.OrganisationalUnitRepository.GetAllEntities();
            List<ADDomain> allDomains = await unitOfWork.ADDomainRepository.GetAllEntities();
            List<int> collectedSiteIds = new List<int>();
            string user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            foreach (OrganisationalUnit OU in allOUs)
            {
                collectedSiteIds.Clear();

                ADDomain domain = allDomains.Find(x => x.ADDomainId == OU.ADDomainRefId);
                string domainName = domain.Name;

                if (OU.Computers != null)
                {
                    foreach (var computer in OU.Computers)
                    {
                        if (pingService.isPingable(computer.IpAddress))
                        {
                            if (!collectedSiteIds.Contains(computer.SiteRefId))
                            {
                                computer.PingSuccessful = true;
                                

                                OU.HasReachableComputer = true;
                                unitOfWork.OrganisationalUnitRepository.Update(OU);

                                collectedSiteIds.Add(computer.SiteRefId);

                                getRSoP(computer.ComputerName + "." + domainName,
                                                                OU.OrganisationalUnitId, computer.SiteRefId,
                                                                user);
                            }

                            computer.isSysmonRunning = sysmonService.isSysmonRunning(serviceName, user,
                                computer.ComputerName,
                                domainName);

                            unitOfWork.ComputerRepository.Update(computer);
                        }
                    }
                }

                await unitOfWork.SaveChangesAsync();
            }
        }


        public void getRSoP(string computerpath, int ouId, int siteId, string user)
        {
            try
            {
                GPRsop gpRsop = new GPRsop(RsopMode.Logging, "");
                gpRsop.LoggingMode = LoggingMode.Computer;
                gpRsop.LoggingComputer = computerpath;
                gpRsop.LoggingUser = user;
                gpRsop.CreateQueryResults();
                gpRsop.GenerateReportToFile(ReportType.Xml, ConfigurationManager.AppSettings["ReceivedRSoP"] + "\\" + "Ou_" + ouId + "-Site_" + siteId + ".xml");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void clearOldRsops()
        {
            string[] filePaths = Directory.GetFiles(ConfigurationManager.AppSettings["ReceivedRSoP"]);
            foreach (string filePath in filePaths)
            {

                File.Delete(filePath);
            }
                
        }
    }
}