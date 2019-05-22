using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using NLog.LayoutRenderers.Wrappers;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Services
{
    public class SysmonService : ISysmonService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPingService pingService;

        public SysmonService(IUnitOfWork unitOfWork, IPingService pingService)
        {
            this.unitOfWork = unitOfWork;
            this.pingService = pingService;
        }

        public async Task sysmonCheck(string serviceName)
        {
        List<OrganisationalUnit> allOUs = await unitOfWork.OrganisationalUnitRepository.GetAllEntities();
        List<ADDomain> allDomains = await unitOfWork.ADDomainRepository.GetAllEntities();

        foreach (OrganisationalUnit OU in allOUs)
            {
                foreach (var computer in OU.Computers)
                {
                    var domain = OU.ADDomain;

                    
                    if (pingService.isPingable(computer.IpAddress))
                    {
                        computer.PingSuccessful = true;
                        computer.isSysmonRunning =  isSysmonRunning(
                            serviceName, System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString(),
                            computer.ComputerName, domain.Name);
                    }
                }

                await unitOfWork.SaveChangesAsync();
            }
        }


        public bool isSysmonRunning(string serviceName, string user, string computerName, string domain)
        {
            ConnectionOptions op = new ConnectionOptions();
            ManagementScope scope = new ManagementScope(@"\\" + computerName +"."+ domain + "\\root\\cimv2", op);
            scope.Connect();
            ManagementPath path = new ManagementPath("Win32_Service");
            ManagementClass services = new ManagementClass(scope, path, null);

            foreach (var service in services.GetInstances())
            { 
                if (service.GetPropertyValue("Name").ToString().Equals(serviceName) && service.GetPropertyValue("State").ToString().ToLower().Equals("running"))
                { 
                    return true;
                }
            }
            return false;
        }

    }
}
