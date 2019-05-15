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
    public class RSoPResultViewModel : ViewModelBase, IRSoPResultViewModel
    {
        private readonly IUnitOfWork unityOfWork;

        public string GISS{ get; set; }
        public int RefId{ get; set; }

        [Obsolete("Only for design data", true)]
        public RSoPResultViewModel()
        {
            if (!this.IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public RSoPResultViewModel(IUnitOfWork unityOfWork)
        {
            this.unityOfWork = unityOfWork;
        }

        private Rsop loadRsopOfRsopPot()
        {
            var rsopPot = unityOfWork.RSoPPotRepository.GetByID(RefId);
            return rsopPot.Rsops.FirstOrDefault();

        }

        private List<SecuirtySettingsParser> loadSettings()
        {
            List<SecuirtySettingsParser> settings = new List<SecuirtySettingsParser>();
            var rsop = loadRsopOfRsopPot();
            foreach (var setting in rsop.AuditSettings)
            {
                SecuirtySettingsParser parsedSetting = new SecuirtySettingsParser();
                parsedSetting.Setting = setting.SubcategoryName;
                parsedSetting.Value = setting.CurrentSettingValue.ToString();
                parsedSetting.Target = setting.TargetSettingValue.ToString();

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
                SecuirtySettingsParser parsedSetting = new SecuirtySettingsParser();
                parsedSetting.Setting = setting.Name;
                parsedSetting.Value = setting.CurrentState;
                parsedSetting.Target = setting.TargetState;

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
                SecuirtySettingsParser parsedSetting = new SecuirtySettingsParser();
                parsedSetting.Setting = setting.Name;
                parsedSetting.Value = setting.CurrentValue.Name;
                parsedSetting.Target = setting.TargetValue.Name;

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
                SecuirtySettingsParser parsedSetting = new SecuirtySettingsParser();
                parsedSetting.Setting = setting.Description;
                parsedSetting.Value = setting.CurrentDisplay.DisplayBoolean;
                parsedSetting.Target = setting.TargetDisplay.DisplayBoolean;

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

        public List<SecuirtySettingsParser> AuditSettings
        {
            get => loadSettings();
        }

        private List<string> loadOUs()
        {
            List<string> ous = new List<string>();
            var rsopPot = unityOfWork.RSoPPotRepository.GetByID(RefId);
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
    }
}
 