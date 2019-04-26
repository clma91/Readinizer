using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class TreeNode
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public List<TreeNode> ChildNodes { get; set; } = new List<TreeNode>();
    }
}
