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
        public static Gpo ReadinizerGoodGpo = new Gpo
        {
            Name = "Readinizer Good GPO",
            Enabled = "true",
            GpoIdentifier = new Identifier
            {
                Id = "1"
            },
            Link = new List<Link>
            {
                new Link
                {
                    SOMPath = ""
                }
            },
        };

        public static Gpo ReadinizerBadGpo = new Gpo
        {
            Name = "Readinizer Bad GPO",
            Enabled = "true",
            GpoIdentifier = new Identifier
            {
                Id = "2"
            },
            Link = new List<Link>
            {
                new Link
                {
                    SOMPath = ""
                }
            },
        };

        public static ADDomain ReadinizerDomain = new ADDomain
        {
            Name = "readinizer.ch",
            IsAvailable = true,
            IsForestRoot = true,
            IsTreeRoot = false,
            ParentId = null,
        };

        public static OrganisationalUnit ReadinizerOu = new OrganisationalUnit
        {
            Name = "Readinizer Ou",
            ADDomain = ReadinizerDomain,
            Computers = new List<Computer>
            {
                new Computer
                {
                    ComputerName = "ReadinizerWS01",
                    IpAddress = "10.0.0.9",
                    IsDomainController = false,
                    isSysmonRunning = true,
                    PingSuccessful = true,
                }
            },
            HasReachableComputer = true,
            LdapPath = "test\\path",
        };

        public static OrganisationalUnit ReadinizerSalesOu = new OrganisationalUnit
        {
            Name = "Readinizer Sales Ou",
            ADDomain = ReadinizerDomain,
            Computers = new List<Computer>
            {
                new Computer
                {
                    ComputerName = "ReadinizerWS02",
                    IpAddress = "10.0.1.9",
                    IsDomainController = false,
                    isSysmonRunning = true,
                    PingSuccessful = true,
                }
            },
            HasReachableComputer = true,
            LdapPath = "test\\path",
        };

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
            ComputerAccountManagementSuccess,
            OtherAccountManagementEventsSuccess,
            SecurityGroupManagementSuccess,
            UserAccountManagementSuccessAndFailure,
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
            AuditPolicyChangeSuccess,
            MPSSVCRuleLevelPolicyChangeSuccess,
            NonSensitivePrivilegeUseSuccessAndFailure,
            SensitivePrivilegeUseSuccessAndFailure,
            SecuritySystemExtensionSuccess,
            SystemIntegritySuccessAndFailure,
            DirectoryServiceChangesSuccess
        };

        public static List<Policy> RecommendedPolicies = new List<Policy>
        {
            IncludeCommandLineEnabled,
            ModuleLoggingEnabled,
            ScriptBlockLoggingEnabled
        };

        public static List<RegistrySetting> RecommendedRegistrySettings = new List<RegistrySetting>
        {
            LsassEnabled,
            LsaProtectionEnabled
        };

        public static List<SecurityOption> RecommendedSecurityOptions = new List<SecurityOption>
        {
            ForceAuditPolicyEnabled
        };
    }   
}
