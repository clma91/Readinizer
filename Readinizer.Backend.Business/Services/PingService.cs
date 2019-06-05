using System.Net.NetworkInformation;
using Readinizer.Backend.Business.Interfaces;


namespace Readinizer.Backend.Business.Services
{
    public class PingService : IPingService
    {
        public bool isPingable(string ipAddress)
        {
            bool isPingable;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                var reply = pinger.Send(ipAddress, 500);
                isPingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
            finally
            {
                pinger?.Dispose();
            }

            return isPingable;
        }

    }
}