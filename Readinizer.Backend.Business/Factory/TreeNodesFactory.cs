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

            var rootDomain = domains.FirstOrDefault();
            var rsopPots = GetRsopPotsOfDomain(rootDomain);
            rootDomain.RsopsPercentage = rsopPots.Min(x => x.Rsops.Min(y => y.RsopPercentage));
            unitOfWork.ADDomainRepository.Update(rootDomain);

            root.Type = rootDomain.IsForestRoot ? "Forest Root Domain: " : "Domain: ";
            root.Name = rootDomain.Name;
            root.RsopPotPercentage = rootDomain.RsopsPercentage ?? 0.0;
            foreach (var rsopPot in rsopPots)
            {
                var rsopPotOfDomain = new TreeNode
                {
                    Type = "RSoP Pot: ",
                    Name = rsopPot.Name,
                    RsopPotPercentage = rsopPot.Rsops.First().RsopPercentage
                };
                root.ChildNodes.Add(rsopPotOfDomain);
            }

            BuildTree(root, rootDomain.SubADDomains);

            await unitOfWork.SaveChangesAsync();
            tree.Add(root);
            return tree;
        }

        private void BuildTree(TreeNode root, List<ADDomain> domains)
        {
            if (domains != null)
            {
                foreach (var domain in domains)
                {
                    var rsopPots = GetRsopPotsOfDomain(domain);
                    domain.RsopsPercentage = rsopPots.Min(x => x.Rsops.Min(y => y.RsopPercentage)); ;
                    unitOfWork.ADDomainRepository.Update(domain);

                    var child = new TreeNode
                    {
                        Type = "Domain: ",
                        Name = domain.Name,
                        RsopPotPercentage = domain.RsopsPercentage ?? 0.0
                    };
                    foreach (var rsopPot in rsopPots)
                    {
                        var rsopPotOfDomain = new TreeNode
                        {
                            Type = "RSoP Pot: ",
                            Name = rsopPot.Name,
                            RsopPotPercentage = rsopPot.Rsops.First().RsopPercentage
                        };
                        child.ChildNodes.Add(rsopPotOfDomain);
                    }

                    root.ChildNodes.Add(child);
                    BuildTree(child, domain.SubADDomains);
                }
            }
        }

        private List<RsopPot> GetRsopPotsOfDomain(ADDomain domain)
        {
            var rsopsOfDomain = domain.Rsops;
            var rsopPots = new HashSet<RsopPot>();

            foreach (var rsop in rsopsOfDomain)
            {
                rsopPots.Add(unitOfWork.RSoPPotRepository.GetByID(rsop.RsopPotRefId));
            }

            rsopPots.Remove(null);
            return rsopPots.ToList();
        }
    }
}
