using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class TreeNode
    {
        public Dictionary<string, int> TypeRefIdDictionary { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double RsopPotPercentage { get; set; }

        public bool IsRSoP { get; set; }

        public List<TreeNode> ChildNodes { get; set; } = new List<TreeNode>();

        public List<OrganisationalUnit> OrganisationalUnits { get; set; }
    }
}
