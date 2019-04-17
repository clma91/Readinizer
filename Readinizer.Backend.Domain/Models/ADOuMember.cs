﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class ADOuMember
    {
        public int ADOuMemberId { get; set; }

        public string ComputerName { get; set; }

        public bool IsDomainController { get; set; }

        public int OURefId { get; set; }

        public ADOrganisationalUnit ADOrganisationalUnit { get; set; }

        public string IpAddress { get; set; }

        public bool PingSuccessfull { get; set; }
    }
}
