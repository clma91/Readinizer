using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;

namespace Readinizer.Frontend.ViewModels
{
    public class TreeStructureResultViewModel : ViewModelBase, ITreeStructureResultViewModel
    {
        private readonly IADDomainService adDomainService;
        private readonly IOrganisationalUnitService adOrganisationalUnitService;
        private readonly IComputerService adOuMemberService;

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

        public TreeStructureResultViewModel(IADDomainService adDomainService, IOrganisationalUnitService adOrganisationalUnitService, IComputerService adOuMemberService)
        {
            this.adDomainService = adDomainService;
            this.adOrganisationalUnitService = adOrganisationalUnitService;
            this.adOuMemberService = adOuMemberService;
        }

        private async void Discover()
        {
            Messenger.Default.Send(new ChangeView(typeof(StartUpViewModel)));
        }
    }
}
