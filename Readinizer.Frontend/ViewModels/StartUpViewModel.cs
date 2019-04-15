using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.Business.Services;
using Readinizer.Backend.Domain.Models;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;

namespace Readinizer.Frontend.ViewModels
{
    public class StartUpViewModel : ViewModelBase, IStartUpViewModel
    {
        private readonly IADDomainService adDomainService;
        private readonly IADOrganisationalUnitService adOrganisationalUnitService;
        private readonly IADOuMemberService adOuMemberService;

        public ADDomain Domain;

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

        public StartUpViewModel(IADDomainService adDomainService, IADOrganisationalUnitService adOrganisationalUnitService, IADOuMemberService adOuMemberService)
        {
            this.adDomainService = adDomainService;
            this.adOrganisationalUnitService = adOrganisationalUnitService;
            this.adOuMemberService = adOuMemberService;
            CanDiscover = true;
            CanAnalyse = true;
        }

        private async void Discover()
        {
            await Task.Run(() => adDomainService.SearchAllDomains());
        }

        private async void Analyse()
        {
            await Task.Run(() => adOrganisationalUnitService.GetAllOrganisationalUnits());
            await Task.Run(() => adOuMemberService.GetMembersOfOu());
            ShowTreeStructureResult();
        }

        private void ShowTreeStructureResult()
        {
            Messenger.Default.Send(new ChangeView(typeof(TreeStructureResultViewModel)));
        }
    }
}
