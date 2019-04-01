using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class ADOrganisationalUnit
    {
        public ADOrganisationalUnit(string Name)
        {
            this.Name = Name;
        }

        public int Id { get; set; }

        private string name;
        public string Name { get; set; }
    }
}
