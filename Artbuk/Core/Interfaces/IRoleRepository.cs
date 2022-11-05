using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface IRoleRepository
    {
        Role GetById(Guid id);
        Role GetRoleIdAtUser();
        Role GetRoleIdAtAdmin();
        string GetRoleNameById(Guid roleId);
    }
}
