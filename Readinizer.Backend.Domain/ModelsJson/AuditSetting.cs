﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Domain.ModelsJson
{
    public class AuditSetting
    {
        public int AuditSettingId { get; set; }

        public int RsopRefId { get; set; }

        public Rsop Rsop { get; set; }

        [JsonProperty("SubCategoryName")]
        public string SubcategoryName { get; set; }

        [JsonProperty("PolicyTarget")]
        public string PolicyTarget { get; set; }

        [JsonProperty("SettingValue")]
        public AuditSettingValue TargetSettingValue { get; set; }

        public AuditSettingValue CurrentSettingValue { get; set; }

        public bool IsPresent { get; set; }
    }
}
