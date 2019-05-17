using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.Business.Services;
using Readinizer.Backend.DataAccess;
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
        private readonly ISysmonService sysmonService;
        private readonly IRSoPPotService rSoPPotService;

        private ICommand discoverCommand;
        public ICommand DiscoverCommand => discoverCommand ?? (discoverCommand = new RelayCommand(() => this.Discover(), () => this.CanDiscover));
        private ICommand analyseCommand;
        public ICommand AnalyseCommand => analyseCommand ?? (analyseCommand = new RelayCommand(() => this.Analyse(), () => this.CanAnalyse));

        public bool CanDiscover { get; private set; }
        public bool CanAnalyse { get; private set; }

        private bool subdomainsChecked;
        public bool SubdomainsChecked
        {
            get => subdomainsChecked;
            set
            {
                Set(ref subdomainsChecked, value);
            }
        }

        private bool sysmonChecked;
        public bool SysmonChecked
        {
            get => sysmonChecked; 
            set
            {
                Set(ref sysmonChecked, value);
            }
        }

        private string sysmonName;
        public string SysmonName
        {
            get => sysmonName;
            set { Set(ref sysmonName, value); }
        }

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
            this.sysmonService = sysmonService;
            this.analysisService = analysisService;
            this.rSoPPotService = rSoPPotService;
            CanDiscover = true;
            CanAnalyse = true;
        }
 
        private async void Discover()
        {

            if (domainName != null && adDomainService.IsDomainInForest(domainName))
            {

                if (subdomainsChecked)
                {
                    try
                    {
                        var emptyVm = new EmptyViewModel();
                        DialogHost.Show(emptyVm);
                    
                        await Task.Run(() => adDomainService.SearchAllDomains(domainName));
                        await Task.Run(() => siteService.SearchAllSites());
                        await Task.Run(() => organisationalUnitService.GetAllOrganisationalUnits());
                        await Task.Run(() => computerService.GetComputers());

                        DialogHost.CloseDialogCommand.Execute(null, null);
                        Messenger.Default.Send(new SnackbarMessage("Collected all domains"));
                        CanAnalyse = true;
                    }
                    catch (Exception e)
                    {
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        Messenger.Default.Send(new SnackbarMessage(e.Message));
                    }
                }
                else
                {
                    try
                    {
                        var emptyVm = new EmptyViewModel();
                        DialogHost.Show(emptyVm);

                        await Task.Run(() => adDomainService.AddThisDomain(domainName));
                        await Task.Run(() => siteService.SearchAllSites());
                        await Task.Run(() => organisationalUnitService.GetAllOrganisationalUnits());
                        await Task.Run(() => computerService.GetComputers());

                        DialogHost.CloseDialogCommand.Execute(null, null);
                        Messenger.Default.Send(new SnackbarMessage("Collected all domains"));
                        CanAnalyse = true;
                    }
                    catch (Exception e)
                    {
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        Messenger.Default.Send(new SnackbarMessage(e.Message));
                    }
                }
            }
            else
            {
                Messenger.Default.Send(
                    new SnackbarMessage("Could not find specified domain in this forest"));
            }
        }

        private async void Analyse()
        {

            if (sysmonChecked)
            {
                if (sysmonName == null || sysmonName == "")
                {
                    sysmonName = "Sysmon";
                }

                try
                {
                    ShowSpinnerView();
                    await Task.Run(() => rSoPService.getRSoPOfReachableComputersAndCheckSysmon(sysmonName));
                    await Task.Run(() => analysisService.Analyse());
                    await Task.Run(() => rSoPPotService.GenerateRsopPots());
                    ShowTreeStructureResult();
                }
                catch (Exception e)
                {
                    ShowStartView();
                    Messenger.Default.Send(new SnackbarMessage(e.Message));
                }
            }
            else
            {
                try
                {
                    ShowSpinnerView();
                    await Task.Run(() => rSoPService.getRSoPOfReachableComputers());
                    await Task.Run(() => analysisService.Analyse());
                    await Task.Run(() => rSoPPotService.GenerateRsopPots());
                    ShowTreeStructureResult();
                }
                catch (Exception e)
                {
                    ShowStartView();
                    Messenger.Default.Send(new SnackbarMessage(e.Message));
                }


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

        private void ShowStartView()
        {
            Messenger.Default.Send(new ChangeView(typeof(StartUpViewModel)));
        }

        private void ShowDomainResultView(int refId)
        {
            Messenger.Default.Send(new ChangeView(typeof(DomainResultViewModel), refId));
        }

    }
}
