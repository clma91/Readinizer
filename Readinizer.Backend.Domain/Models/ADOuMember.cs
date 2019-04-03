using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class ADOuMember
    {
        public int Id { get; set; }

        public string ComputerName { get; set; }

        public bool IsDomainController { get; set; }

        public int OURefId { get; set; }
    }
}
