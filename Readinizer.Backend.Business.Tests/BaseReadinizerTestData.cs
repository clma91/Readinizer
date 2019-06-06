using System;
using System.Collections.Generic;
using System.Globalization;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Business.Tests
{
    public class BaseReadinizerTestData : BaseReadinizerTestSettingData
    {
        public static Rsop GoodRsopRedinizerOu = new Rsop
        {
            Gpos = new List<Gpo>
            {
                ReadinizerGoodGpo
            },
            Domain = ReadinizerDomain,
            OrganisationalUnit = ReadinizerOu,
            AuditSettings = new List<AuditSetting>
            {
                KerberosAuthServiceSuccessAndFailure,
                LogoffSuccess,
                ProcessCreationSuccess
            },
            RegistrySettings = new List<RegistrySetting>
            {
                LsaProtectionEnabled
            },
            Policies = new List<Policy>
            {
                IncludeCommandLineEnabled,
                ModuleLoggingEnabled
            },
            SecurityOptions = new List<SecurityOption>
            {
                ForceAuditPolicyEnabled
            }
        };

        public static Rsop BadRsopRedinizerOu = new Rsop
        {
            Gpos = new List<Gpo>
            {
                ReadinizerGoodGpo
            },
            Domain = ReadinizerDomain,
            OrganisationalUnit = ReadinizerOu,
            AuditSettings = new List<AuditSetting>
            {
                KerberosAuthServiceFailure,
                LogoffFailure,
                ProcessCreationNoAuditing
            },
            RegistrySettings = new List<RegistrySetting>
            {
                LsaProtectionNotEnabled
            },
            Policies = new List<Policy>
            {
                IncludeCommandLineNotEnabled,
                ModuleLoggingNotEnabled
            },
            SecurityOptions = new List<SecurityOption>
            {
                ForceAuditPolicyNotEnabled
            }
        };

        public static Rsop GoodRsopSalesOu = new Rsop
        {
            Gpos = new List<Gpo>
            {
                ReadinizerGoodGpo
            },
            Domain = ReadinizerDomain,
            OrganisationalUnit = ReadinizerSalesOu,
            AuditSettings = new List<AuditSetting>
            {
                KerberosAuthServiceSuccessAndFailure,
                LogoffSuccess,
                ProcessCreationSuccess
            },
            RegistrySettings = new List<RegistrySetting>
            {
                LsaProtectionEnabled
            },
            Policies = new List<Policy>
            {
                IncludeCommandLineEnabled,
                ModuleLoggingEnabled
            },
            SecurityOptions = new List<SecurityOption>
            {
                ForceAuditPolicyEnabled
            }
        };

        public static Rsop BadRsopSalesOu = new Rsop
        {
            Gpos = new List<Gpo>
            {
                ReadinizerBadGpo
            },
            Domain = ReadinizerDomain,
            OrganisationalUnit = ReadinizerSalesOu,
            AuditSettings = new List<AuditSetting>
            {
                KerberosAuthServiceFailure,
                ProcessCreationNoAuditing,
                LogoffFailure
            },
            RegistrySettings = new List<RegistrySetting>
            {
                LsaProtectionNotEnabled
            },
            Policies = new List<Policy>
            {
                IncludeCommandLineNotEnabled,
                ModuleLoggingNotEnabled
            },
            SecurityOptions = new List<SecurityOption>
            {
                ForceAuditPolicyNotEnabled
            }
        };

        public static List<Rsop> RsopsNotEqualDifferentOus = new List<Rsop>
        {
            GoodRsopRedinizerOu,
            BadRsopSalesOu
        };

        public static List<Rsop> RsopsNotEqualSameOus = new List<Rsop>
        {
            GoodRsopRedinizerOu,
            BadRsopRedinizerOu
        };

        public static List<Rsop> RsopsEqualDifferentOus = new List<Rsop>
        {
            GoodRsopRedinizerOu,
            GoodRsopSalesOu
        };

        public static List<Rsop> RsopsEqualSameOus = new List<Rsop>
        {
            GoodRsopRedinizerOu,
            GoodRsopRedinizerOu
        };

        public static RsopPot RsopPotGoodReadinizerOu = new RsopPot
        {
            Rsops = new List<Rsop>
            {
                GoodRsopRedinizerOu
            },
            Name = "1",
            DateTime = "6.6.19",
            Domain = ReadinizerDomain,
        };

        public static List<AuditSetting> RecommendedAuditSettings = new List<AuditSetting>
        {
            KerberosAuthServiceSuccessAndFailure,
            KerberosServiceTicketOpSuccessAndFailure,
            ComputerAccountManagementSuccessAndFailure,
            OtherAccountManagementEventsSuccessAndFailure,
            SecurityGroupManagementSuccessAndFailure,
            UserAccountManagementSuccessAndFailure,
            DirectoryServiceChangesSuccess,
            ProcessCreationSuccess,
            ProcessTerminationSuccess,
            AccountLockoutFailure,
            GroupMembershipSuccess,
            LogoffSuccess,
            LogonSuccessAndFailure,
            OtherLogonLogoffEventsSuccessAndFailure,
            SpecialLogonSuccess,
            FileShareSuccessAndFailure,
            FileSystemSuccessAndFailure,
            HandleManipulationSuccess,
            KernelObjectSuccessAndFailure,
            OtherObjectAccessEventsSuccessAndFailure,
            RegistrySuccessAndFailure,
            SAMSuccessAndFailure,
            AuditPolicyChangeSuccessAndFailure,
            MPSSVCRuleLevelPolicyChangeSuccess,
            NonSensitivePrivilegeUseSuccessAndFailure,
            SensitivePrivilegeUseSuccessAndFailure,
            SecuritySystemExtensionSuccessAndFailure,
            SystemIntegritySuccessAndFailure,
        };
    }   
}
