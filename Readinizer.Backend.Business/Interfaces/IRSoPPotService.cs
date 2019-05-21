using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Interfaces
{
    public interface IRSoPPotService
    {
        Task GenerateRsopPots();

        Task UpdateRsopPots(List<Rsop> rsops);
    }
}
