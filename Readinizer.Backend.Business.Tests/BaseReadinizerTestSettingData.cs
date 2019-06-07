using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.Domain.ModelsJson;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Business.Tests
{
    public class BaseReadinizerTestSettingData
    {
        public static AuditSetting KerberosAuthServiceSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit Kerberos Authentication Service",
            PolicyTarget = "Account Logon",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting KerberosServiceTicketOpSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit Kerberos Service Ticket Operations",
            PolicyTarget = "Account Logon",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting ComputerAccountManagementSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Computer Account Management",
            PolicyTarget = "Account Management",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting OtherAccountManagementEventsSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Other Account Management Events",
            PolicyTarget = "Account Management",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting SecurityGroupManagementSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Security Group Management",
            PolicyTarget = "Account Management",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting UserAccountManagementSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit User Account Management",
            PolicyTarget = "Account Management",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting ProcessCreationSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Process Creation",
            PolicyTarget = "Detailed Tracking",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting ProcessTerminationSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Process Termination",
            PolicyTarget = "Detailed Tracking",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting AccountLockoutFailure = new AuditSetting
        {
            SubcategoryName = "Audit Account Lockout",
            PolicyTarget = "Logon/Logoff",
            TargetSettingValue = AuditSettingValue.Failure,
            CurrentSettingValue = AuditSettingValue.Failure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting GroupMembershipSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Group Membership",
            PolicyTarget = "Logon/Logoff",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting LogoffSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Logoff",
            PolicyTarget = "Logon/Logoff",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting LogonSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit Logon",
            PolicyTarget = "Logon/Logoff",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting OtherLogonLogoffEventsSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit Other Logon/Logoff Events",
            PolicyTarget = "Logon/Logoff",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting SpecialLogonSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Special Logon",
            PolicyTarget = "Logon/Logoff",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting FileShareSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit File Share",
            PolicyTarget = "Object Access",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting FileSystemSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit File System",
            PolicyTarget = "Object Access",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting HandleManipulationSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Handle Manipulation",
            PolicyTarget = "Object Access",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting KernelObjectSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit Kernel Object",
            PolicyTarget = "Object Access",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting OtherObjectAccessEventsSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit Other Object Access Events",
            PolicyTarget = "Object Access",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting RegistrySuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit Registry",
            PolicyTarget = "Object Access",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting SAMSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit SAM",
            PolicyTarget = "Object Access",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting AuditPolicyChangeSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Audit Policy Change",
            PolicyTarget = "Policy Change",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting MPSSVCRuleLevelPolicyChangeSuccess = new AuditSetting
        {
            SubcategoryName = "Audit MPSSVC Rule-Level Policy Change",
            PolicyTarget = "Policy Change",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting NonSensitivePrivilegeUseSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit Non Sensitive Privilege Use",
            PolicyTarget = "Privilege Use",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting SensitivePrivilegeUseSuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit Sensitive Privilege Use",
            PolicyTarget = "Privilege Use",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting SecuritySystemExtensionSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Security System Extension",
            PolicyTarget = "System",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting SystemIntegritySuccessAndFailure = new AuditSetting
        {
            SubcategoryName = "Audit System Integrity",
            PolicyTarget = "System",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.SuccessAndFailure,
            IsPresent = true,
            GpoId = "1"
        };
        public static AuditSetting DirectoryServiceChangesSuccess = new AuditSetting
        {
            SubcategoryName = "Audit Directory Service Changes",
            PolicyTarget = "DS Access",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Success,
            IsPresent = true,
            GpoId = "NoGpoId"
        };

        public static AuditSetting KerberosAuthServiceFailure = new AuditSetting
        {
            SubcategoryName = "Audit Kerberos Authentication Service",
            PolicyTarget = "Account Logon",
            TargetSettingValue = AuditSettingValue.SuccessAndFailure,
            CurrentSettingValue = AuditSettingValue.Failure,
            IsPresent = true,
            GpoId = ""
        };
        
        public static AuditSetting ProcessCreationNoAuditing = new AuditSetting
        {
            SubcategoryName = "Audit Process Creation",
            PolicyTarget = "Detailed Tracking",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.NoAuditing,
            IsPresent = true,
            GpoId = ""
        };
        
        public static AuditSetting LogoffFailure = new AuditSetting
        {
            SubcategoryName = "Audit Logoff",
            PolicyTarget = "Logon/Logoff",
            TargetSettingValue = AuditSettingValue.Success,
            CurrentSettingValue = AuditSettingValue.Failure,
            IsPresent = true,
            GpoId = ""
        };

        public static RegistrySetting LsaProtectionEnabled = new RegistrySetting
        {
            Name = "LSA Protection",
            Path = "Computer Configuration\\Policies\\Administrative Templates\\SCM: Pass the Hash Mitigations",
            KeyPath = "SYSTEM\\CurrentControlSet\\Control\\Lsa",
            TargetValue = new Value
            {
                Name = "RunAsPPL",
                Number = "1"
            },
            CurrentValue = new Value
            {
                Name = "RunAsPPL",
                Number = "1"
            },
            IsPresent = true,
            GpoId = "1"
        };

        public static RegistrySetting LsaProtectionNotEnabled = new RegistrySetting
        {
            Name = "LSA Protection",
            Path = "Computer Configuration\\Policies\\Administrative Templates\\SCM: Pass the Hash Mitigations",
            KeyPath = "SYSTEM\\CurrentControlSet\\Control\\Lsa",
            TargetValue = new Value
            {
                Name = "RunAsPPL",
                Number = "1"
            },
            CurrentValue = new Value
            {
                Element = new Element(),
                Name = "Undefined",
                Number = "Undefined"
            },
            IsPresent = false,
            GpoId = "2"
        };

        public static RegistrySetting LsassEnabled = new RegistrySetting
        {
            Name = "Lsass.exe audit mode",
            Path = "Computer Configuration\\Policies\\Administrative Templates\\SCM: Pass the Hash Mitigations",
            KeyPath = "Software\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options\\LSASS.exe",
            TargetValue = new Value
            {
                Name = "AuditLevel",
                Number = "8"
            },
            CurrentValue = new Value
            {
                Name = "AuditLevel",
                Number = "8"
            },
            IsPresent = true,
            GpoId = "1"
        };

        public static Policy IncludeCommandLineEnabled = new Policy
        {
            Name = "Include command line in process creation events",
            TargetState = "Enabled",
            CurrentState = "Enabled",
            Category = "System/Audit Process Creation",
            IsPresent = true,
            GpoId = "1"
        };

        public static Policy IncludeCommandLineNotEnabled = new Policy
        {
            Name = "Include command line in process creation events",
            TargetState = "Enabled",
            CurrentState = "Disabled",
            Category = "System/Audit Process Creation",
            IsPresent = false,
            GpoId = "2"
        };

        public static Policy ModuleLoggingEnabled = new Policy
        {
            Name = "Turn on Module Logging",
            TargetState = "Enabled",
            CurrentState = "Enabled",
            Category = "Windows Components/Windows PowerShell",
            IsPresent = true,
            ModuleNames = new ModuleNames
            {
                State = "Enabled",
                ValueElementData = "*"
            },
            GpoId = "1"
        };

        public static Policy ModuleLoggingNotEnabled = new Policy
        {
            Name = "Turn on Module Logging",
            TargetState = "Enabled",
            CurrentState = "Disabled",
            Category = "Windows Components/Windows PowerShell",
            IsPresent = true,
            ModuleNames = new ModuleNames
            {
                State = "undefined",
                ValueElementData = "undefined"
            },
            GpoId = "2"
        };

        public static Policy ScriptBlockLoggingEnabled = new Policy
        {
            Name = "Turn on PowerShell Script Block Logging",
            TargetState = "Enabled",
            CurrentState = "Enabled",
            Category = "Windows Components/Windows PowerShell",
            IsPresent = true,
            GpoId = "1"
        };

        public static SecurityOption ForceAuditPolicyEnabled = new SecurityOption
        {
            Description = "Force Audit Policy",
            Path =
                "Computer Configuration\\Policies\\Windows Settings\\Security Settings\\Local Policies\\Security Options",
            KeyName = "MACHINE\\System\\CurrentControlSet\\Control\\Lsa\\SCENoApplyLegacyAuditPolicy",
            TargetSettingNumber = "1",
            CurrentSettingNumber = "1",
            TargetDisplay = new Display
            {
                Name =
                    "Audit: Force audit policy subcategory settings (Windows Vista or later) to override audit policy category settings",
                DisplayBoolean = "true"
            },
            CurrentDisplay = new Display
            {
                Name =
                    "Audit: Force audit policy subcategory settings (Windows Vista or later) to override audit policy category settings",
                DisplayBoolean = "true"
            },
            IsPresent = true,
            GpoId = "1"
        };

        public static SecurityOption ForceAuditPolicyNotEnabled = new SecurityOption
        {
            Description = "Force Audit Policy",
            Path =
                "Computer Configuration\\Policies\\Windows Settings\\Security Settings\\Local Policies\\Security Options",
            KeyName = "MACHINE\\System\\CurrentControlSet\\Control\\Lsa\\SCENoApplyLegacyAuditPolicy",
            TargetSettingNumber = "1",
            CurrentSettingNumber = "",
            TargetDisplay = new Display
            {
                Name =
                    "Audit: Force audit policy subcategory settings (Windows Vista or later) to override audit policy category settings",
                DisplayBoolean = "false"
            },
            CurrentDisplay = new Display
            {
                Name = "Undefined",
                DisplayBoolean = "Undefined"
            },
            IsPresent = false,
            GpoId = "2"
        };
    }
}
