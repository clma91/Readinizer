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
        public int Id { get; set; }

        public string Name { get; set; }
        public string LdapPath { get; set; }

        //[ForeignKey("ADDomain")]
        public int DomainRefId { get; set; }
    }
}
