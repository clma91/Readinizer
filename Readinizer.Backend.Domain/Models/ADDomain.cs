using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class ADDomain
    {
        public ADDomain(string Name, bool isTreeRoot, bool isForestRoot)
        {
            this.Name = Name;
            this.isTreeRoot = isTreeRoot;
            this.isForstRoot = isForstRoot;
        }

        public int Id { get; set; }

        private string name;
        public string Name { get; set; }

        public bool isTreeRoot { get; set; }

        public bool isForstRoot { get; set; }
    }
}
