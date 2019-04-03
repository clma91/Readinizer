using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class ADOrganisationalUnit
    {
        public ADOrganisationalUnit(string Name, string LdapPath, int DomainRefId)
        {
            this.Name = Name;
            this.LdapPath = LdapPath;
            this.DomainRefId = DomainRefId;
        }

        public int Id { get; set; }

        private string name;
        public string Name { get; set; }
        public string LdapPath { get; set; }

        //[ForeignKey("ADDomain")]
        public int DomainRefId { get; set; }
    }
}
