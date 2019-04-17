﻿using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using Readinizer.Backend.Domain.Exceptions;
using AD = System.DirectoryServices.ActiveDirectory;

namespace Readinizer.Backend.Business.Services
{
    public class ADDomainService : IADDomainService
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IUnityOfWork unityOfWork;

        public ADDomainService(IUnityOfWork unityOfWork)
        {
            this.unityOfWork = unityOfWork;
        }

        public async Task SearchAllDomains()
        {
            var domains = new List<AD.Domain>();
            var treeDomains = new List<AD.Domain>();
            var treeDomainsWithChildren = new List<AD.Domain>();

            try
            {
                var forestRootDomain = Forest.GetCurrentForest().RootDomain;
                var domainTrusts = forestRootDomain.GetAllTrustRelationships();

                foreach (TrustRelationshipInformation domainTrust in domainTrusts)
                {
                    if (domainTrust.TrustType.Equals(AD.TrustType.TreeRoot))
                    {
                        var treeDomain =
                            AD.Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain,
                                domainTrust.TargetName));
                        treeDomains.Add(treeDomain);
                    }
                }

                AddAllChildDomains(forestRootDomain, domains);
            }
            catch (UnauthorizedAccessException accessException)
            {
                var message = "Invalid access rights for domain call";
                logger.Error(accessException, message);
                throw new InvalidAuthenticationException(message);
            }
            catch (ActiveDirectoryServerDownException severDownException)
            {
                var message = $"Server {severDownException.Name} is down";
                logger.Error(severDownException, message);
                throw new InvalidAuthenticationException(message);
            }
            catch (Exception e)
            {
                var message = "Unkown error occurred";
                logger.Error(e, message);
                throw new InvalidAuthenticationException(message);
            }

            foreach (var treeDomain in treeDomains)
            {
                AddAllChildDomains(treeDomain, treeDomainsWithChildren);
            }

            var models = MapToDomainModel(domains, treeDomainsWithChildren);

            unityOfWork.ADDomainRepository.AddRange(models);

            await unityOfWork.SaveChangesAsync();
        }
        
        private static void AddAllChildDomains(AD.Domain root, List<AD.Domain> domains)
        {
            domains.Add(root);

            for (var i = 0; i < root.Children.Count; ++i)
            {
                AddAllChildDomains(root.Children[i], domains);
            }
        }

        private static List<ADDomain> MapToDomainModel(List<AD.Domain> domains, List<AD.Domain> treeDomains)
        {
            var models = domains.Select(x => new ADDomain { Name = x.Name, ADSubDomains = new List<ADDomain>() }).ToList();
            var treeModels = treeDomains.Select(x => new ADDomain {Name = x.Name, IsTreeRoot = true, ADSubDomains = new List<ADDomain>()}).ToList();

            AddSubDomains(domains, models);
            AddSubDomains(treeDomains, treeModels);

            var allModels = models.Union(treeModels).ToList();

            var root = allModels.FirstOrDefault(m => IsForestRoot(m.Name));
            if (root != null)
            {
                root.IsForestRoot = true;
                root.ADSubDomains.AddRange(treeModels);
            }

            return allModels;
        }

        private static void AddSubDomains(List<AD.Domain> domains, List<ADDomain> models)
        {
            foreach (var adDomain in models)
            {
                var children = domains.ToArray().Where(d => d.Parent?.Name == adDomain.Name).Select(x => x.Name);
                adDomain.ADSubDomains.AddRange(models.Where(m => children.Contains(m.Name)));
            }
        }

        private static bool IsForestRoot(string domainName)
        {
            return Forest.GetCurrentForest().RootDomain.Name.Equals(domainName);
        }

        public bool IsDomainInForest(string fullyQualifiedDomainName)
        {
            var isInForest = false;

            foreach (AD.Domain domain in Forest.GetCurrentForest().Domains)
            {
                if (domain.Name.Equals(fullyQualifiedDomainName))
                {
                    isInForest = true;
                }
            }

            return isInForest;
        }
    }
}
