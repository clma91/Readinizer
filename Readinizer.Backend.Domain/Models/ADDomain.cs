using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class ADDomain
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool isTreeRoot { get; set; }

        public bool isForstRoot { get; set; }
    }
}
