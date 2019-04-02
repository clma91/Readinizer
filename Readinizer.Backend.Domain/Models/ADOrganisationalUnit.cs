using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class ADOrganisationalUnit
    {
        public ADOrganisationalUnit(string Name, string LdapPath)
        {
            this.Name = Name;
            this.LdapPath = LdapPath;
        }

        public int Id { get; set; }

        private string name;
        public string Name { get; set; }
        public string LdapPath { get; set; }
    }
}
