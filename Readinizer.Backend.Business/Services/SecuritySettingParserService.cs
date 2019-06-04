using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson;

namespace Readinizer.Backend.Business.Services
{
    public class SecuritySettingParserService : ISecuritySettingParserService
    {
        private readonly IUnitOfWork unitOfWork;
        
        public SecuritySettingParserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<SecuritySettingsParsed>> ParseSecuritySettings(int RefId, string type)
        {
            Rsop rsop = new Rsop();
            if (type.Equals("RsopPot"))
            {
                var rsopPot = unitOfWork.RsopPotRepository.GetByID(RefId);
                rsop = rsopPot.Rsops.FirstOrDefault();
            }
            else
            {
                rsop = unitOfWork.RsopRepository.GetByID(RefId);

            }

            var GPOs = await unitOfWork.GpoRepository.GetAllEntities();
            
            List<SecuritySettingsParsed> settings = new List<SecuritySettingsParsed>();
            foreach (var setting in rsop.AuditSettings)
            {
                var parsedSetting = SecuritySettingFactory(setting.SubcategoryName, setting.CurrentSettingValue.ToString(), setting.TargetSettingValue.ToString());
                var gopId = setting.GpoId;

                ParseSecuritySetting(gopId, parsedSetting, GPOs);

                settings.Add(parsedSetting);
            }

            foreach (var setting in rsop.Policies)
            {
                var parsedSetting = SecuritySettingFactory(setting.Name, setting.CurrentState, setting.TargetState);
                var gopId = setting.GpoId;

                ParseSecuritySetting(gopId, parsedSetting, GPOs);

                settings.Add(parsedSetting);
            }


            foreach (var setting in rsop.RegistrySettings)
            {
                var parsedSetting = SecuritySettingFactory(setting.Name, setting.CurrentValue.Name, setting.TargetValue.Name);
                var gopId = setting.GpoId;

                ParseSecuritySetting(gopId, parsedSetting, GPOs);

                settings.Add(parsedSetting);
            }

            foreach (var setting in rsop.SecurityOptions)
            {
                var parsedSetting = SecuritySettingFactory(setting.Description, setting.CurrentDisplay.DisplayBoolean, setting.TargetDisplay.DisplayBoolean);
                var gopId = setting.GpoId;

                ParseSecuritySetting(gopId, parsedSetting, GPOs);

                settings.Add(parsedSetting);
            }


            return settings;
        }

        private static SecuritySettingsParsed SecuritySettingFactory(string setting, string value, string target)
        {
            return new SecuritySettingsParsed
            {
                Setting = setting,
                Value = value,
                Target = target
            };
        }

        private static void ParseSecuritySetting(string gopId, SecuritySettingsParsed parsedSetting, List<Gpo> GPOs)
        {
            if (gopId.Equals("NoGpoId"))
            {
                parsedSetting.GPO = "-";
            }
            else
            {
                parsedSetting.GPO = GPOs.Find(x => x.GpoPath.GpoIdentifier.Id.Equals(gopId)).Name;
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
        }
    }
}
