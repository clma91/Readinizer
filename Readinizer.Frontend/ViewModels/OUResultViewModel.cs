using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Schema;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;

namespace Readinizer.Frontend.ViewModels
{
    public class OUResultViewModel : ViewModelBase, IOUResultViewModel
    {
        private readonly IUnitOfWork unityOfWork;
        private readonly ISecuritySettingParserService securitySettingParserService;

        private ICommand backCommand;
        public ICommand BackCommand => backCommand ?? (backCommand = new RelayCommand(() => this.Back(), () => this.CanBack));

        public bool CanBack { get; private set; }

        public int RefId{ get; set; }
        private Rsop rsop { get => unityOfWork.RsopRepository.GetByID(RefId); }
        public string Ou { get => rsop.OrganisationalUnit.Name; }
        private List<SecuritySettingsParsed> securitySettings;
        public List<SecuritySettingsParsed> SecuritySettings
        {
            get => securitySettings;
            set => Set(ref securitySettings, value);
        }

        [Obsolete("Only for design data", true)]
        public OUResultViewModel()
        {
            if (!this.IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public OUResultViewModel(IUnitOfWork unityOfWork, ISecuritySettingParserService securitySettingParserService)
        {
            this.securitySettingParserService = securitySettingParserService;
            this.unityOfWork = unityOfWork;
            CanBack = true;
        }

        public void Load() => LoadSettings();

        private async void LoadSettings()
        {
            SecuritySettings = await securitySettingParserService.ParseSecuritySettings(RefId);
            RaisePropertyChanged(nameof(SecuritySettings));
        }

        private void ShowPotView(int potRefId)
        {
            Messenger.Default.Send(new ChangeView(typeof(RSoPResultViewModel), potRefId));

        }

        private void Back()
        {
            ShowPotView(rsop.RsopPotRefId.GetValueOrDefault());
        }
    }
}
 