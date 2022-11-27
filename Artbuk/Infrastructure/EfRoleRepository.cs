using Artbuk.Core.Interfaces;
using Artbuk.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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

        public Guid GetUserRoleId()
        {
            return GetRoleIdByName(Constants.RoleNames.User);
        }
        public Guid GetAdminRoleId()
        {
            return GetRoleIdByName(Constants.RoleNames.Admin);
        }

        public Guid GetRoleIdByName(string name)
        {
            var role = _dbContext.Roles
                .FirstOrDefault(i => i.Name == name);

            if (role == null)
            {
                throw new Exception($"Role with name {name} not exists.");
            }

            return role.Id;
        }

        public string GetRoleNameById(Guid roleId)
        {
            var role = GetById(roleId);

            if (role == null)
            {
                throw new Exception($"Role with Id {roleId} not exists.");
            }

            return role.Name;
        }
    }
}
