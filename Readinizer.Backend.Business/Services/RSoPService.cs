﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Management;
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
            List<OrganisationalUnit> allOUs = await unitOfWork.OrganisationalUnitRepository.GetAllEntities();
            List<ADDomain> allDomains = await unitOfWork.ADDomainRepository.GetAllEntities();
            List<int> collectedSiteIds = new List<int>();
            foreach (OrganisationalUnit OU in allOUs)
            {
                collectedSiteIds.Clear();

                var domain = allDomains.Find(x => x.ADDomainId == OU.ADDomainRefId);

                foreach (var computer in OU.Computers)
                {
                    if (!collectedSiteIds.Contains(computer.SiteRefId) && pingService.isPingable(computer.IpAddress))
                    {
                        computer.PingSuccessfull = true;
                        unitOfWork.ComputerRepository.Update(computer);

                        OU.HasReachableComputer = true;
                        unitOfWork.OrganisationalUnitRepository.Update(OU);

                        collectedSiteIds.Add(computer.SiteRefId);

                        getRSoP(computer.ComputerName + "." + domain.Name,
                            computer.ComputerName,
                            System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    }

                }

                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task getRSoPOfReachableComputersAndCheckSysmon()
        {
            List<OrganisationalUnit> allOUs = await unitOfWork.OrganisationalUnitRepository.GetAllEntities();
            List<ADDomain> allDomains = await unitOfWork.ADDomainRepository.GetAllEntities();
            List<int> collectedSiteIds = new List<int>();
            string user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            foreach (OrganisationalUnit OU in allOUs)
            {
                collectedSiteIds.Clear();

                ADDomain domain = allDomains.Find(x => x.ADDomainId == OU.ADDomainRefId);
                string domainName = domain.Name;

                foreach (var computer in OU.Computers)
                {
                    if (pingService.isPingable(computer.IpAddress))
                    {
                        if (!collectedSiteIds.Contains(computer.SiteRefId))
                        {
                            computer.PingSuccessfull = true;
                            unitOfWork.ComputerRepository.Update(computer);

                            OU.HasReachableComputer = true;
                            unitOfWork.OrganisationalUnitRepository.Update(OU);

                            collectedSiteIds.Add(computer.SiteRefId);

                            getRSoP(computer.ComputerName + "." + domainName,
                                computer.ComputerName,
                                user);
                        }

                        computer.isSysmonRunning = sysmonService.isSysmonRunning(user, computer.ComputerName,
                            domainName);
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
                test.GenerateReportToFile(ReportType.Xml,
                    (AppDomain.CurrentDomain.BaseDirectory + "\\RSOP\\" + computername + ".xml"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
       }   
    }
}