using System;
using System.Collections.Generic;
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
            get
            {
                if (treeNodes == null)
                {
                    treeNodes = new List<TreeNode>();
                }

                return treeNodes;
            }
            set => Set(ref treeNodes, value);
        }



        private List<ADDomain> subDomins;

        public List<ADDomain> SubDomains
        {
            get => subDomins;
            set => Set(ref subDomins, value);
        }

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
            TreeNodes = await treeNodesFactory.CreateTree();
        }

        private async void Discover()
        {
            Messenger.Default.Send(new ChangeView(typeof(StartUpViewModel)));
        }
    }
}
