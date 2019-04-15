﻿using Readinizer.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Backend.DataAccess.Interfaces
{
    public interface IADOuMemberRepository
    {
        void Add(ADOuMember member);
        Task SaveChangesAsync();
    }
}