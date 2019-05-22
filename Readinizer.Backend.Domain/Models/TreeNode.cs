using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Readinizer.Backend.Domain.Models
{
    public class TreeNode
    {
        [JsonIgnore]
        public Dictionary<string, int> TypeRefIdDictionary { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double AnalysisPercentage { get; set; }

        [JsonIgnore]
        public bool IsRSoP { get; set; }

        public List<TreeNode> ChildNodes { get; set; } = new List<TreeNode>();

        public List<OrganisationalUnit> OrganisationalUnits { get; set; }

        public Rsop Rsop { get; set; }
    }
}
