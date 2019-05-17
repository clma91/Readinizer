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

        private ICommand backCommand;
        public ICommand BackCommand => backCommand ?? (backCommand = new RelayCommand(() => this.Back(), () => this.CanBack));

        public bool CanBack { get; private set; }


        
        public int RefId{ get; set; }
        private Rsop rsop { get => unityOfWork.RSoPRepository.GetByID(RefId); }
        public string Ou { get => rsop.OrganisationalUnit.Name; }

        [Obsolete("Only for design data", true)]
        public OUResultViewModel()
        {
            if (!this.IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public OUResultViewModel(IUnitOfWork unityOfWork)
        {
            this.unityOfWork = unityOfWork;
            CanBack = true;
        }

        private List<SecuiritySettingsParserOU> loadSettings()
        {
            var GPOs = unityOfWork.GpoRepository.GetAllEntities().Result;
            List<SecuiritySettingsParserOU> settings = new List<SecuiritySettingsParserOU>();
            foreach (var setting in rsop.AuditSettings)
            {
                SecuiritySettingsParserOU parsedSetting = new SecuiritySettingsParserOU();
                parsedSetting.Setting = setting.SubcategoryName;
                parsedSetting.Value = setting.CurrentSettingValue.ToString();
                parsedSetting.Target = setting.TargetSettingValue.ToString();
                if (setting.GpoId.Equals("NoGpoId"))
                {
                    parsedSetting.GPO = "-";
                }
                else
                {
                    parsedSetting.GPO = GPOs.Find(x => x.GpoPath.GpoIdentifier.Id.Equals(setting.GpoId)).Name;
                }

                if (parsedSetting.Value.Equals(parsedSetting.Target))
                {
                    parsedSetting.Icon = "Check";
                    parsedSetting.Color = "Green";
                }
                else if (parsedSetting.Value.Equals("NoAuditing"))
                {
                    parsedSetting.Icon = "Exclamation";
                    parsedSetting.Color = "Orange";
                }
                else
                {
                    parsedSetting.Icon = "Close";
                    parsedSetting.Color = "Red";
                }

                settings.Add(parsedSetting);
            }

            foreach (var setting in rsop.Policies)
            {
                SecuiritySettingsParserOU parsedSetting = new SecuiritySettingsParserOU();
                parsedSetting.Setting = setting.Name;
                parsedSetting.Value = setting.CurrentState;
                parsedSetting.Target = setting.TargetState;
                if (setting.GpoId.Equals("NoGpoId"))
                {
                    parsedSetting.GPO = "-";
                }
                else
                {
                    parsedSetting.GPO = GPOs.Find(x => x.GpoPath.GpoIdentifier.Id.Equals(setting.GpoId)).Name;
                }

                if (parsedSetting.Value.Equals(parsedSetting.Target))
                {
                    parsedSetting.Icon = "Check";
                    parsedSetting.Color = "Green";
                }
                else
                {
                    parsedSetting.Icon = "Close";
                    parsedSetting.Color = "Red";
                }

                settings.Add(parsedSetting);
            }


            foreach (var setting in rsop.RegistrySettings)
            {
                SecuiritySettingsParserOU parsedSetting = new SecuiritySettingsParserOU();
                parsedSetting.Setting = setting.Name;
                parsedSetting.Value = setting.CurrentValue.Name;
                parsedSetting.Target = setting.TargetValue.Name;
                if (setting.GpoId.Equals("NoGpoId"))
                {
                    parsedSetting.GPO = "-";
                }
                else
                {
                    parsedSetting.GPO = GPOs.Find(x => x.GpoPath.GpoIdentifier.Id.Equals(setting.GpoId)).Name;
                }

                if (parsedSetting.Value.Equals(parsedSetting.Target))
                {
                    parsedSetting.Icon = "Check";
                    parsedSetting.Color = "Green";
                }
                else
                {
                    parsedSetting.Icon = "Close";
                    parsedSetting.Color = "Red";
                }

                settings.Add(parsedSetting);
            }

            foreach (var setting in rsop.SecurityOptions)
            {
                SecuiritySettingsParserOU parsedSetting = new SecuiritySettingsParserOU();
                parsedSetting.Setting = setting.Description;
                parsedSetting.Value = setting.CurrentDisplay.DisplayBoolean;
                parsedSetting.Target = setting.TargetDisplay.DisplayBoolean;
                if (setting.GpoId.Equals("NoGpoId"))
                {
                    parsedSetting.GPO = "-";
                }
                else
                {
                    parsedSetting.GPO = GPOs.Find(x => x.GpoPath.GpoIdentifier.Id.Equals(setting.GpoId)).Name;
                }

                if (parsedSetting.Value.Equals(parsedSetting.Target))
                {
                    parsedSetting.Icon = "Check";
                    parsedSetting.Color = "Green";

                }
                else if (parsedSetting.Value.Equals("NotDefined"))
                {
                    parsedSetting.Icon = "Exclamation";
                    parsedSetting.Color = "Orange";
                }
                else
                {
                    parsedSetting.Icon = "Close";
                    parsedSetting.Color = "Red";
                }

                settings.Add(parsedSetting);
            }


            return settings;
        }

        public List<SecuiritySettingsParserOU> AuditSettings
        {
            get => loadSettings();
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
 