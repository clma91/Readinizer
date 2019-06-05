using System.Threading.Tasks;

namespace Readinizer.Backend.Business.Interfaces
{
    public interface IRSoPService
    {

        Task getRSoPOfReachableComputers();

        Task getRSoPOfReachableComputersAndCheckSysmon(string serviceName);
    }
}
