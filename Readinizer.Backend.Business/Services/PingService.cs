using System;
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