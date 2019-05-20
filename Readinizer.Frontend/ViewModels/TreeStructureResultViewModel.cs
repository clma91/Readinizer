﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;

namespace Readinizer.Frontend.ViewModels
{
    public class TreeStructureResultViewModel : ViewModelBase, ITreeStructureResultViewModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITreeNodesFactory treeNodesFactory;

        private ADDomain rootDomain;
        public ADDomain RootDomain
        {
            get => rootDomain;
            set => Set(ref rootDomain, value);
        }

        private ObservableCollection<TreeNode> treeNodes;
        public ObservableCollection<TreeNode> TreeNodes
        {
            get => treeNodes ?? (treeNodes = new ObservableCollection<TreeNode>());
            set => Set(ref treeNodes, value);
        }

        private string selecteDomain;
        public string SelectedDomain
        {
            get => selecteDomain;
            set { Set(ref selecteDomain, value); }
        }

        public ObservableCollection<ObservableCollection<OrganisationalUnit>> OUsWithoutRSoP { get; set; } = new ObservableCollection<ObservableCollection<OrganisationalUnit>>();

        private ICommand discoverCommand;
        public ICommand DiscoverCommand => discoverCommand ?? (discoverCommand = new RelayCommand(() => this.Discover()));

        private ICommand detailCommand;
        public ICommand DetailCommand => detailCommand ?? (detailCommand = new RelayCommand<Dictionary<string, int>>(param => this.ShowDetail(param)));

        private void ShowDetail(Dictionary<string, int> param)
        {
            if (param.First().Key.Equals("Domain"))
            {
                Messenger.Default.Send(new ChangeView(typeof(DomainResultViewModel), param.First().Value));
            }
            else
            {
                Messenger.Default.Send(new ChangeView(typeof(RSoPResultViewModel), param.First().Value));
            }
        }

        [Obsolete("Only for design data", true)]
        public TreeStructureResultViewModel()
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public TreeStructureResultViewModel(ITreeNodesFactory treeNodesFactory, IUnitOfWork unitOfWork)
        {
            this.treeNodesFactory = treeNodesFactory;
            this.unitOfWork = unitOfWork;
            TreeNodes = new ObservableCollection<TreeNode>();
        }

        public async void BuildTree()
        {
            await SetOusWithoutRSoPs();
            TreeNodes = await treeNodesFactory.CreateTree();
        }

        private async Task SetOusWithoutRSoPs()
        {
            var allOrganisationalUnits = await unitOfWork.OrganisationalUnitRepository.GetAllEntities();
            var ousWithoutRsoP = allOrganisationalUnits.FindAll(x => !x.HasReachableComputer);
            AddOu(ousWithoutRsoP.First());

            foreach (var organisationalUnit in ousWithoutRsoP.Skip(1))
            {
                bool found = false;

                foreach (var sortedOu in OUsWithoutRSoP)
                {
                    if (sortedOu.ToList().Exists(x => x.ADDomain.Name.Equals(organisationalUnit.ADDomain.Name)))
                    {
                        sortedOu.Add(organisationalUnit);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    AddOu(organisationalUnit);
                }
            }

            void AddOu(OrganisationalUnit ou)
            {
                OUsWithoutRSoP.Add(new ObservableCollection<OrganisationalUnit> { ou });
            }

        }

        private void Discover()
        {
            Messenger.Default.Send(new ChangeView(typeof(SysmonResultViewModel)));
        }

    }
}
