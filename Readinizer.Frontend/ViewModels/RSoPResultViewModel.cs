using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
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
using Unity.Injection;

namespace Readinizer.Frontend.ViewModels
{
    public class RSoPResultViewModel : ViewModelBase, IRSoPResultViewModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISecuritySettingParserService securitySettingParserService;

        private ICommand backCommand;
        public ICommand BackCommand => backCommand ?? (backCommand = new RelayCommand(() => this.Back()));

        public RsopPot rsopPot { get; set; }

        public string GISS
        {
            get => rsopPot.Name;
        }

        public int RefId{ get; set; }

        private List<SecuritySettingsParsed> securitySettings;
        public List<SecuritySettingsParsed> SecuritySettings
        {
            get => securitySettings;
            set => Set(ref securitySettings, value);
        }


        [Obsolete("Only for design data", true)]
        public RSoPResultViewModel()
        {
            if (!this.IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public RSoPResultViewModel(IUnitOfWork unitOfWork, ISecuritySettingParserService securitySettingParserService)
        {
            this.unitOfWork = unitOfWork;
            this.securitySettingParserService = securitySettingParserService;
        }


        public void Load() => LoadSettings();


        private async void LoadSettings()
        {
            SecuritySettings = await securitySettingParserService.ParseSecuritySettings(RefId);
            RaisePropertyChanged(nameof(SecuritySettings));
        }

        private List<string> loadOUs()
        {
            List<string> ous = new List<string>();
            var rsops = rsopPot.Rsops;
            foreach (var rsop in rsops)
            {
                ous.Add(rsop.OrganisationalUnit.Name);
            }

            return ous;
        }

        public List<string> OUsInGISS
        {
            get => loadOUs();
        }

        private async Task<List<OrganisationalUnit>> ousAsync()
        {
            var ous = await unitOfWork.OrganisationalUnitRepository.GetAllEntities();
            return ous;
        }

        private string rsop;
        public string Rsop
        {
            get { return rsop; }
            set
            {
                rsop = value;

                List<Rsop> rsopList = rsopPot.Rsops.ToList();
                int rsopID = rsopList.Find(x => x.OrganisationalUnit.Name.Equals(rsop)).RsopId;
                ShowOUView(rsopID);
                rsop = null;
            }
        }


        private void ShowOUView(int rsopRefId)
        {
            Messenger.Default.Send(new ChangeView(typeof(OUResultViewModel), rsopRefId));
        }

        private void ShowDomainView(int domainRefId)
        {
            Messenger.Default.Send(new ChangeView(typeof(DomainResultViewModel), domainRefId));
        }

        private void Back()
        {
            ShowDomainView(rsopPot.Domain.ADDomainId);
        }
    }
}
 