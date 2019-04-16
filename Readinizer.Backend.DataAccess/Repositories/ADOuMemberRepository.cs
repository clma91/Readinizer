using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;


namespace Readinizer.Backend.DataAccess.Repositories
{
    public class ADOuMemberRepository : IADOuMemberRepository
    {
        private readonly ReadinizerDbContext context;

        public ADOuMemberRepository()
        {
            this.context = new ReadinizerDbContext();
        }

        public void Add(ADOuMember member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            context.ADOuMembers.Add(member);
        }

        public Task<List<ADOuMember>> getAllOUMembers()
        {
            return context.ADOuMembers.ToListAsync<ADOuMember>();
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
