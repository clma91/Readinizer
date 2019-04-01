using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Readinizer.Backend.Business.Services;
using Readinizer.Backend.Domain.Models;
using Readinizer.Frontend.Commands;

namespace Readinizer.Frontend.ViewModels
{
    class StartUpViewModel : INotifyPropertyChanged
    {
        private readonly ADDomainService domainService;
        private readonly ADOrganisationalUnitService organisationalUnitService;
        public ADDomain Domain;
        private ICommand discoverCommand;
        public ICommand DiscoverCommand => discoverCommand ?? (discoverCommand = new RelayCommand(() => this.Save(), () => this.CanSave));
        private ICommand analyseCommand;
        public ICommand AnalyseCommand => analyseCommand ?? (analyseCommand = new RelayCommand(() => this.Load(), () => this.CanLoad));


        private bool canSave;
        public bool CanSave
        {
            get { return canSave; }
            private set { canSave = value; }
        }
        private bool canLoad;
        public bool CanLoad
        {
            get { return canLoad; }
            private set { canLoad = value; }
        }

        private string domainName;
        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; OnPropertyChanged("DomainName"); }
        }


        public StartUpViewModel()
        {
            this.domainService = new ADDomainService();
            this.organisationalUnitService = new ADOrganisationalUnitService();
            CanSave = true;
            CanLoad = true;
        }

        private void Save()
        {
            
        }

        private void Load()
        {
            
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
