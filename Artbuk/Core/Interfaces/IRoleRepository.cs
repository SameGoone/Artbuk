using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface IRoleRepository
    {
        Role GetById(Guid id);
        Guid GetUserRoleId();
        Guid GetAdminRoleId();
        string GetRoleNameById(Guid roleId);
        Guid GetRoleIdByName(string name);
    }
}
