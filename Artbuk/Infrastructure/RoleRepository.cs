using Artbuk.Models;
using System;

namespace Artbuk.Infrastructure
{
    public class RoleRepository
    {
        private readonly ArtbukContext _dbContext;

        public RoleRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Role? GetById(Guid id)
        {
            return _dbContext.Roles
                .FirstOrDefault(i => i.Id == id);
        }

        public Guid? GetUserRoleId()
        {
            return GetRoleIdByName(Constants.RoleNames.User);
        }
        public Guid? GetAdminRoleId()
        {
            return GetRoleIdByName(Constants.RoleNames.Admin);
        }

        public Guid? GetRoleIdByName(string name)
        {
            var role = _dbContext.Roles
                .FirstOrDefault(i => i.Name == name);

            return role != null
                ? role.Id
                : null;
        }

        public string? GetRoleNameById(Guid roleId)
        {
            var role = GetById(roleId);

            return role != null 
                ? role.Name 
                : null;
        }
    }
}
