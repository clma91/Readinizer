using System.Collections.Generic;
using Readinizer.Backend.Domain.Models;
using Readinizer.Backend.Domain.ModelsJson;
using Readinizer.Backend.Domain.ModelsJson.HelperClasses;

namespace Readinizer.Backend.Business.Tests
{
    public class BaseReadinizerTestData
    {
        public static List<Rsop> Rsops = new List<Rsop>
        {
            new Rsop
            {
                Gpos = new List<Gpo>
                {
                    new Gpo
                    {
                        Name = "Readinizer GPO",
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
                    }
                },
                Domain = new ADDomain
                {
                    Name = "readinizer.ch",
                    IsAvailable = true,
                    IsForestRoot = true,
                    IsTreeRoot = false,
                    ParentId = null,
                },
                AuditSettings = new List<AuditSetting>
                {
                    new AuditSetting
                    {
                        SubcategoryName = "Audit Kerberos Authentication Service",
                        PolicyTarget = "Account Logon",
                        TargetSettingValue = AuditSettingValue.SuccessAndFailure,
                        CurrentSettingValue = AuditSettingValue.NoAuditing,
                        IsPresent = true,
                        GpoId = "1"
                    },
                    new AuditSetting
                    {
                        SubcategoryName = "Audit Process Creation",
                        PolicyTarget = "Detailed Tracking",
                        TargetSettingValue = AuditSettingValue.Success,
                        CurrentSettingValue = AuditSettingValue.Failure,
                        IsPresent = true,
                        GpoId = "1"
                    },
                    new AuditSetting
                    {
                        SubcategoryName = "Audit Logoff",
                        PolicyTarget = "Logon/Logoff",
                        TargetSettingValue = AuditSettingValue.Success,
                        CurrentSettingValue = AuditSettingValue.NoAuditing,
                        IsPresent = false,
                        GpoId = "2"
                    }
                },
                RegistrySettings = new List<RegistrySetting>
                {
                    new RegistrySetting
                    {
                        Name = "LSA Protection",
                        Path = "Computer Configuration\\Policies\\Administrative Templates\\SCM: Pass the Hash Mitigations",
                        KeyPath = "SYSTEM\\CurrentControlSet\\Control\\Lsa",
                        TargetValue = new Value {
                            Name = "RunAsPPL",
                            Number = "1"
                        },
                        CurrentValue = new Value {
                            Name = "RunAsPPL",
                            Number = "1"
                        },
                        IsPresent = true,
                    }
                },
                Policies = new List<Policy>
                {
                    new Policy
                    {
                        Name = "Include command line in process creation events",
                        TargetState = "Enabled",
                        CurrentState = "Enabled",
                        Category = "System/Audit Process Creation",
                        IsPresent = true,
                    },
                    new Policy
                    {
                        Name = "Turn on Module Logging",
                        TargetState = "Enabled",
                        CurrentState = "Enabled",
                        Category = "Windows Components/Windows PowerShell",
                        IsPresent = true,
                        ModuleNames = new ModuleNames {
                            State = "Enabled",
                            ValueElementData = "*"
                        }
                    }
                },
                SecurityOptions = new List<SecurityOption>
                {
                    new SecurityOption
                    {
                        Description = "Force Audit Policy",
                        Path = "Computer Configuration\\Policies\\Windows Settings\\Security Settings\\Local Policies\\Security Options",
                        KeyName = "MACHINE\\System\\CurrentControlSet\\Control\\Lsa\\SCENoApplyLegacyAuditPolicy",
                        TargetSettingNumber = "1",
                        CurrentSettingNumber = "1",
                        TargetDisplay = new Display {
                            Name = "Audit: Force audit policy subcategory settings (Windows Vista or later) to override audit policy category settings",
                            DisplayBoolean = "true"
                        },
                        CurrentDisplay = new Display
                        {
                            Name = "Audit: Force audit policy subcategory settings (Windows Vista or later) to override audit policy category settings",
                            DisplayBoolean = "true"
                        }
                    }
                }
            },
            new Rsop
            {
                Gpos = new List<Gpo>
                {
                    new Gpo
                    {
                        Name = "Readinizer Wrong GPO",
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
                    }
                },
                Domain = new ADDomain
                {
                    Name = "readinizer.ch",
                    IsAvailable = true,
                    IsForestRoot = true,
                    IsTreeRoot = false,
                    ParentId = null,
                },
                AuditSettings = new List<AuditSetting>
                {
                    new AuditSetting
                    {
                        SubcategoryName = "Audit Kerberos Authentication Service",
                        PolicyTarget = "Account Logon",
                        TargetSettingValue = AuditSettingValue.SuccessAndFailure,
                        CurrentSettingValue = AuditSettingValue.NoAuditing,
                        IsPresent = false,
                        GpoId = ""
                    },
                    new AuditSetting
                    {
                        SubcategoryName = "Audit Process Creation",
                        PolicyTarget = "Detailed Tracking",
                        TargetSettingValue = AuditSettingValue.Success,
                        CurrentSettingValue = AuditSettingValue.NoAuditing,
                        IsPresent = false,
                        GpoId = ""
                    },
                    new AuditSetting
                    {
                        SubcategoryName = "Audit Logoff",
                        PolicyTarget = "Logon/Logoff",
                        TargetSettingValue = AuditSettingValue.Success,
                        CurrentSettingValue = AuditSettingValue.NoAuditing,
                        IsPresent = false,
                        GpoId = ""
                    }
                },
                RegistrySettings = new List<RegistrySetting>
                {
                    new RegistrySetting
                    {
                        Name = "LSA Protection",
                        Path = "Computer Configuration\\Policies\\Administrative Templates\\SCM: Pass the Hash Mitigations",
                        KeyPath = "SYSTEM\\CurrentControlSet\\Control\\Lsa",
                        TargetValue = new Value {
                            Name = "RunAsPPL",
                            Number = "1"
                        },
                        CurrentValue = new Value {
                            Element = new Element(),
                            Name = "Undefined",
                            Number = "Undefined"
                        },
                        IsPresent = false,
                    }
                },
                Policies = new List<Policy>
                {
                    new Policy
                    {
                        Name = "Include command line in process creation events",
                        TargetState = "Enabled",
                        CurrentState = "Disabled",
                        Category = "System/Audit Process Creation",
                        IsPresent = false,
                    },
                    new Policy
                    {
                        Name = "Turn on Module Logging",
                        TargetState = "Enabled",
                        CurrentState = "Disabled",
                        Category = "Windows Components/Windows PowerShell",
                        IsPresent = true,
                        ModuleNames = null
                    }
                },
                SecurityOptions = new List<SecurityOption>
                {
                    new SecurityOption
                    {
                        Description = "Force Audit Policy",
                        Path = "Computer Configuration\\Policies\\Windows Settings\\Security Settings\\Local Policies\\Security Options",
                        KeyName = "MACHINE\\System\\CurrentControlSet\\Control\\Lsa\\SCENoApplyLegacyAuditPolicy",
                        TargetSettingNumber = "1",
                        CurrentSettingNumber = "",
                        TargetDisplay = new Display {
                            Name = "Audit: Force audit policy subcategory settings (Windows Vista or later) to override audit policy category settings",
                            DisplayBoolean = "true"
                        },
                        CurrentDisplay = new Display
                        {
                            Name = "Undefined",
                            DisplayBoolean = "Undefined"
                        },
                        IsPresent = false
                    }
                }
            }
        };
    }
}
