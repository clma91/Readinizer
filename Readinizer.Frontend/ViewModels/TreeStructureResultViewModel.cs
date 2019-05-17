using System;
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

        private List<TreeNode> treeNodes;
        public List<TreeNode> TreeNodes
        {
            get => treeNodes ?? (treeNodes = new List<TreeNode>());
            set => Set(ref treeNodes, value);
        }

        public ObservableCollection<List<OrganisationalUnit>> OUsWithoutRSoP { get; set; } = new ObservableCollection<List<OrganisationalUnit>>();

        private ICommand discoverCommand;
        public ICommand DiscoverCommand => discoverCommand ?? (discoverCommand = new RelayCommand(() => this.Discover()));


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
            TreeNodes = new List<TreeNode>();
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
                    if (sortedOu.Exists(x => x.ADDomain.Name.Equals(organisationalUnit.ADDomain.Name)))
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
                OUsWithoutRSoP.Add(new List<OrganisationalUnit> { ou });
            }

        }

        private void Discover()
        {
            Messenger.Default.Send(new ChangeView(typeof(StartUpViewModel)));
        }
    }
}
