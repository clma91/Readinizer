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
            var domains = await unitOfWork.ADDomainRepository.GetAllEntities();
            var tree = new List<TreeNode>();
            var root = new TreeNode();

            var rootDomain = domains.Find(x => x.IsForestRoot);

            root.Type = "Forest Root Domain: ";
            root.Name = rootDomain.Name;
            BuildTree(root, rootDomain.SubADDomains);
            
            foreach (var organisationalUnit in rootDomain.OrganisationalUnits)
            {
                root.ChildNodes.Add(new TreeNode { Type = "OU: ", Name = organisationalUnit.Name });
            }
            
            foreach (var domainSite in rootDomain.Sites)
            {
                root.ChildNodes.Add(new TreeNode { Type = "Site: ", Name = domainSite.Name });
            }
            

            tree.Add(root);
            return tree;
        }

        private void BuildTree(TreeNode root, List<ADDomain> domains)
        {
            if (domains != null)
            {
                foreach (var domain in domains)
                {
                    var child = new TreeNode { Type = "Domain: ", Name = domain.Name };
                    foreach (var organisationalUnit in domain.OrganisationalUnits)
                    {
                        if (organisationalUnit.ADDomain.Name.Equals(domain.Name))
                        {
                            var childOu = new TreeNode { Type = "OU: ", Name = organisationalUnit.Name };
                            child.ChildNodes.Add(childOu);
                        }
                    }

                    foreach (var domainSite in domain.Sites)
                    {
                        if (domainSite.Domains.Contains(domain))
                        {
                            var childSite = new TreeNode { Type = "Site: ", Name = domainSite.Name };
                            child.ChildNodes.Add(childSite);
                        }
                    }
                    root.ChildNodes.Add(child);
                    BuildTree(child, domain.SubADDomains);
                }

            }
        }
    }
}
