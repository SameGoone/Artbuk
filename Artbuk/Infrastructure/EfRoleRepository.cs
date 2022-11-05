using Artbuk.Core.Interfaces;
using Artbuk.Models;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Infrastructure
{
    public class EfRoleRepository : IRoleRepository
    {
        private readonly ArtbukContext _dbContext;

        public EfRoleRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Role GetById(Guid id)
        {
            return _dbContext.Roles
                .FirstOrDefault(i => i.Id == id);
        }

        public Role GetRoleIdAtUser()
        {
            return _dbContext.Roles
                .FirstOrDefault(i => i.Name == Constants.RoleNames.User);
        }
        public Role GetRoleIdAtAdmin()
        {
            return _dbContext.Roles
                .FirstOrDefault(i => i.Name == Constants.RoleNames.Admin);
        }

        public string GetRoleNameById(Guid roleId)
        {
            return _dbContext.Roles
                .Where(r => r.Id == roleId)
                .Select(r => r.Name)
                .Single();
        }
    }
}
