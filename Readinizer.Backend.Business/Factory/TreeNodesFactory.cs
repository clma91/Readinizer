using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Factory
{
    public class TreeNodesFactory : ITreeNodesFactory
    {
        private readonly IUnitOfWork unitOfWork;

        public TreeNodesFactory(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<TreeNode>> CreateTree()
        {
            List<ADDomain> domains = await unitOfWork.ADDomainRepository.GetAllEntities();
            var tree = new List<TreeNode>();
            var root = new TreeNode();

            foreach (var domain in domains)
            {
                if (domain.IsForestRoot)
                {
                    root.Type = "Forest Root Domain: ";
                    root.Name = domain.Name;
                    BuildTree(root, domain.SubADDomains);
                }

                foreach (var organisationalUnit in domain.OrganisationalUnits)
                {
                    root.ChildNodes.Add(new TreeNode {Type = "RSoP-Pot: ", Name = organisationalUnit.Name});
                }
            }

            tree.Add(root);
            return tree;
        }

        private void BuildTree(TreeNode root, List<ADDomain> domains)
        {
            foreach (var domain in domains)
            {
                var child = new TreeNode {Type = "Domain: ", Name = domain.Name};
                foreach (var organisationalUnit in domain.OrganisationalUnits)
                {
                    if (organisationalUnit.ADDomain.Name.Equals(domain.Name))
                    {
                        var childOu = new TreeNode {Type = "RSoP-Pot: ", Name = organisationalUnit.Name};
                        child.ChildNodes.Add(childOu);
                    }
                }
                root.ChildNodes.Add(child);
                BuildTree(child, domain.SubADDomains);
            }
        }
    }
}
