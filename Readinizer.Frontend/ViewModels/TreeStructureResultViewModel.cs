using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
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
        private readonly IDialogService dialogService;
        private readonly IAnalysisService analysisService;
        private readonly IRSoPPotService rSoPPotService;

        private ICommand sysmonCommand;
        public ICommand SysmonCommand => sysmonCommand ?? (sysmonCommand = new RelayCommand(Sysmon));

        private ICommand importRSoPsCommand;
        public ICommand ImportRSoPsCommand => importRSoPsCommand ?? (importRSoPsCommand = new RelayCommand(ImportRSoPs));

        private ICommand detailCommand;
        public ICommand DetailCommand => detailCommand ?? (detailCommand = new RelayCommand<Dictionary<string, int>>(param => ShowDetail(param)));

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

        private ObservableCollection<ObservableCollection<OrganisationalUnit>> ouWithoutRSoP;
        public ObservableCollection<ObservableCollection<OrganisationalUnit>> OUsWithoutRSoP
        {
            get => ouWithoutRSoP ?? (ouWithoutRSoP = new ObservableCollection<ObservableCollection<OrganisationalUnit>>());
            set => Set(ref ouWithoutRSoP, value);
        }

        private ObservableCollection<ADDomain> unavailableDomains;
        public ObservableCollection<ADDomain> UnavailableDomains
        {
            get => unavailableDomains ?? (unavailableDomains = new ObservableCollection<ADDomain>());
            set => Set(ref unavailableDomains, value);
        }

        private string selectDomain;
        public string SelectedDomain
        {
            get => selectDomain;
            set => Set(ref selectDomain, value);
        }

        public string WithSysmon { get; set; }
        
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

        public TreeStructureResultViewModel(ITreeNodesFactory treeNodesFactory, IUnitOfWork unitOfWork, IDialogService dialogService, 
                                            IAnalysisService analysisService, IRSoPPotService rSoPPotService)
        {
            this.treeNodesFactory = treeNodesFactory;
            this.unitOfWork = unitOfWork;
            this.dialogService = dialogService;
            this.analysisService = analysisService;
            this.rSoPPotService = rSoPPotService;
            TreeNodes = new ObservableCollection<TreeNode>();
        }

        public async void BuildTree()
        {
            await SetOusWithoutRSoPs();
            await SetUnavailableDomains();
            if (TreeNodes.Count <= 0)
            {
                TreeNodes = await treeNodesFactory.CreateTree();
            }
            Messenger.Default.Send(new EnableExport());
        }

        private async Task SetUnavailableDomains()
        {
            var allDomains = await unitOfWork.ADDomainRepository.GetAllEntities();
            UnavailableDomains.Clear();

            foreach (var domain in allDomains)
            {
                if (!domain.IsAvailable)
                {
                    UnavailableDomains.Add(domain);
                }
            }
            RaisePropertyChanged(nameof(UnavailableDomains));
        }

        private async Task SetOusWithoutRSoPs()
        {
            var allOrganisationalUnits = await unitOfWork.OrganisationalUnitRepository.GetAllEntities();
            var ousWithoutRsoP = allOrganisationalUnits.FindAll(x => !x.HasReachableComputer);
            AddOu(ousWithoutRsoP.First());
            OUsWithoutRSoP.Clear();

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
            RaisePropertyChanged(nameof(OUsWithoutRSoP));
        }

        private static void Sysmon()
        {
            Messenger.Default.Send(new ChangeView(typeof(SysmonResultViewModel)));
        }

        private async void ImportRSoPs()
        {
            var settings = new OpenFileDialogSettings
            {
                Title = "Import RSoPs of unanalysed Organisational Units",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "XML-file (*.xml)|*.xml|All Files (*.*)|*.*",
                Multiselect = true
            };

            bool? success = dialogService.ShowOpenFileDialog(this, settings);
            if (success == true)
            {
                var importPath = settings.FileName;
                if (settings.CheckPathExists)
                {
                    importPath = Path.GetDirectoryName(importPath);
                    var newRsops = await Task.Run(() => analysisService.Analyse(importPath));
                    if (newRsops != null)
                    {
                        await Task.Run(() => rSoPPotService.UpdateRsopPots(newRsops));
                        TreeNodes.Clear();
                        BuildTree();
                    }
                }
                else
                {
                    Messenger.Default.Send(new SnackbarMessage($"The specified path '{importPath}' does not exist"));
                }
            }
        }
    }
}
