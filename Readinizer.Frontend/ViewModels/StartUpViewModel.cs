using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.Business.Services;
using Readinizer.Backend.Domain.Models;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;
using SnackbarMessage = Readinizer.Frontend.Messages.SnackbarMessage;

namespace Readinizer.Frontend.ViewModels
{
    public class StartUpViewModel : ViewModelBase, IStartUpViewModel
    {
        private readonly IADDomainService adDomainService;
        private readonly IOrganisationalUnitService organisationalUnitService;
        private readonly IComputerService computerService;
        private readonly ISiteService siteService;
        private readonly IRSoPService rSoPService;
        private readonly IAnalysisService analysisService;
        private readonly IRSoPPotService rSoPPotService;

        private ICommand discoverCommand;
        public ICommand DiscoverCommand => discoverCommand ?? (discoverCommand = new RelayCommand(() => this.Discover(), () => this.CanDiscover));
        private ICommand analyseCommand;
        public ICommand AnalyseCommand => analyseCommand ?? (analyseCommand = new RelayCommand(() => this.Analyse(), () => this.CanAnalyse));

        public bool CanDiscover { get; private set; }
        public bool CanAnalyse { get; private set; }

        private string domainName;
        public string DomainName
        {
            get => domainName;
            set { Set(ref domainName, value); }
        }

        [Obsolete("Only for design data", true)]
        public StartUpViewModel()
        {
            if (!this.IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public StartUpViewModel(IADDomainService adDomainService, ISiteService siteService, IOrganisationalUnitService organisationalUnitService, 
                                IComputerService computerService, IRSoPService rSoPService, IAnalysisService analysisService,
                                IRSoPPotService rSoPPotService)
        {
            this.adDomainService = adDomainService;
            this.siteService = siteService;
            this.organisationalUnitService = organisationalUnitService;
            this.computerService = computerService;
            this.rSoPService = rSoPService;
            this.analysisService = analysisService;
            this.rSoPPotService = rSoPPotService;
            CanDiscover = true;
            CanAnalyse = true;
        }

        private async void Discover()
        {
            try
            {
                var emptyVm = new EmptyViewModel();
                DialogHost.Show(emptyVm);
                await Task.Run(() => adDomainService.SearchAllDomains());
                await Task.Run(() => siteService.SearchAllSites());
                await Task.Run(() => organisationalUnitService.GetAllOrganisationalUnits());
                //await Task.Run(() => computerService.GetComputers());
                DialogHost.CloseDialogCommand.Execute(null, null);

                Messenger.Default.Send(new SnackbarMessage("Collected all domains"));
            }
            catch (Exception e)
            {
                Messenger.Default.Send(new SnackbarMessage(e.Message));
            }
}

        private async void Analyse()
        {
            try
            {
                //await Task.Run(() => rSoPService.getRSoPOfReachableComputers());
                await Task.Run(() => analysisService.Analyse());
                await Task.Run(() => rSoPPotService.GenerateRsopPots());

                Messenger.Default.Send(new SnackbarMessage("Collected all RSoPs"));
                //ShowTreeStructureResult();
            }
            catch (Exception e)
            {
                Messenger.Default.Send(new SnackbarMessage(e.Message));
            }
            
        }

        private void ShowTreeStructureResult()
        {
            Messenger.Default.Send(new ChangeView(typeof(TreeStructureResultViewModel)));
        }

        private void ShowSpinnerView()
        {
            Messenger.Default.Send(new ChangeView(typeof(SpinnerViewModel)));
        }
    }
}
