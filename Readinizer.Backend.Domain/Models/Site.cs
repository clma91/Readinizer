﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.Domain.Models
{
    public class Site
    {
        public int SiteId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<string> Subnets { get; set; }
        
        public virtual ICollection<ADDomain> Domains { get; set; }
    }
}