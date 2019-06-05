namespace Readinizer.Backend.Business.Interfaces
{
    public interface ISysmonService
    {
        bool isSysmonRunning(string serviceName, string user, string computername, string domain);

    }
}
