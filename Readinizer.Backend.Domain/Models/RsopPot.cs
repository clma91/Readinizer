using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class RsopPot
    {
        public string Name { get; set; }

        public OrganisationalUnit OrganisationalUnit { get; set; }

        public List<Rsop> Rsops { get; set; }
    }
}
