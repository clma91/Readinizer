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
        private readonly IRSoPPotService rSoPPotService;

        private ICommand analyseCommand;
        public ICommand AnalyseCommand => analyseCommand ?? (analyseCommand = new RelayCommand(() => Analyse()));

        private bool subdomainsChecked;
        public bool SubdomainsChecked
        {
            get => subdomainsChecked;
            set => Set(ref subdomainsChecked, value);
        }

        private bool sysmonChecked;
        public bool SysmonChecked
        {
            get => sysmonChecked; 
            set => Set(ref sysmonChecked, value);
        }

        private string sysmonName;
        public string SysmonName
        {
            get => sysmonName;
            set => Set(ref sysmonName, value);
        }

        private string domainName;
        public string DomainName
        {
            get => domainName;
            set => Set(ref domainName, value);
        }


        [Obsolete("Only for design data", true)]
        public StartUpViewModel()
        {
            if (!IsInDesignMode)
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
        }
 
        private async void Analyse()
        {
            if (string.IsNullOrEmpty(domainName) || adDomainService.IsDomainInForest(domainName))
            {
                var sysmonVisability = "Hidden";
                try
                {
                    ShowSpinnerView();
                    ChangeProgressText("Looking for Domains...");
                    await Task.Run(() => adDomainService.SearchDomains(domainName, SubdomainsChecked));
                    ChangeProgressText("Looking for Sites...");
                    await Task.Run(() => siteService.SearchAllSites());
                    ChangeProgressText("Looking for Organisation Units...");
                    await Task.Run(() => organisationalUnitService.GetAllOrganisationalUnits());
                    ChangeProgressText("Looking for Computers...");
                    await Task.Run(() => computerService.GetComputers());
                    
                    if (sysmonChecked)
                    {
                        ChangeProgressText("Looking for RSoPs and check if Sysmon is running...");
                        await Task.Run(() => rSoPService.getRSoPOfReachableComputersAndCheckSysmon(sysmonName));
                        sysmonVisability = "Visible";
                    }
                    else
                    {
                        ChangeProgressText("Collecting RSoPs...");
                        await Task.Run(() => rSoPService.getRSoPOfReachableComputers());
                    }
                    ChangeProgressText("Analysing collected RSoPs...");
                    await Task.Run(() => analysisService.Analyse(null));
                    await Task.Run(() => rSoPPotService.GenerateRsopPots());
                    ShowTreeStructureResult(sysmonVisability);
                }
                catch (Exception e)
                {
                    ShowStartView();
                    Messenger.Default.Send(new SnackbarMessage(e.Message));
                }
            }
            else
            {
                Messenger.Default.Send(new SnackbarMessage("Could not find specified domain in this forest"));
            }
        }

        private static void ChangeProgressText(string progressText)
        {
            Messenger.Default.Send(new ChangeProgressText(progressText));
        }

        private static void ShowTreeStructureResult(string visability)
        {
            Messenger.Default.Send(new ChangeView(typeof(TreeStructureResultViewModel), visability));
        }

        private static void ShowSpinnerView()
        {
            Messenger.Default.Send(new ChangeView(typeof(SpinnerViewModel)));
        }

        private static void ShowStartView()
        {
            Messenger.Default.Send(new ChangeView(typeof(StartUpViewModel)));
        }

        private void ShowDomainResultView(int refId)
        {
            Messenger.Default.Send(new ChangeView(typeof(DomainResultViewModel), refId));
        }

    }
}
