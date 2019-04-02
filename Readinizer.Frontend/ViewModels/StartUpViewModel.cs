using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.Business.Services;
using Readinizer.Backend.Domain.Models;
using Readinizer.Frontend.Commands;
using Readinizer.Frontend.Interfaces;

namespace Readinizer.Frontend.ViewModels
{
    class StartUpViewModel : IStartUpViewModel, INotifyPropertyChanged
    {
        private readonly IADDomainService adDomainService;
        private readonly IADOrganisationalUnitService adOrganisationalUnitService;
        public ADDomain Domain;
        private ICommand discoverCommand;
        public ICommand DiscoverCommand => discoverCommand ?? (discoverCommand = new RelayCommand(() => this.Discover(), () => this.CanDiscover));
        private ICommand analyseCommand;
        public ICommand AnalyseCommand => analyseCommand ?? (analyseCommand = new RelayCommand(() => this.Analyse(), () => this.CanAnalyse));


        private bool canDiscover;
        public bool CanDiscover
        {
            get { return canDiscover; }
            private set { canDiscover = value; }
        }
        private bool canAnalyse;
        public bool CanAnalyse
        {
            get { return canAnalyse; }
            private set { canAnalyse = value; }
        }

        private string domainName;
        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; OnPropertyChanged("DomainName"); }
        }

        public StartUpViewModel(IADDomainService adDomainService, IADOrganisationalUnitService adOrganisationalUnitService)
        {
            this.adDomainService = adDomainService;
            this.adOrganisationalUnitService = adOrganisationalUnitService;
            CanDiscover = true;
            CanAnalyse = true;
        }


        private async void Discover()
        {
            await this.adDomainService.SearchAllDomains();
            
        }

        private async void Analyse()
        {
            await this.adOrganisationalUnitService.GetAllOrganisationalUnits(domainName);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
