using Artbuk.Infrastructure;
using System.Security.Claims;

namespace Artbuk
{
    public class Tools
    {
        public static Guid GetUserId(UserRepository userRepository, ClaimsPrincipal user)
        {
            var userName = user.Identity.Name;
            var userEntity = userRepository.GetByLogin(userName);

            if (userEntity == null)
            {
                throw new Exception("Пользователь не найден.");
            }

            return userEntity.Id;
        }

        //public static int GetRoleId(RoleRepository roleRepository, string roleName)
        //{
        //    var role = roleRepository.ListAsync().Result.FirstOrDefault(r => r.Name == roleName);
        //    if (role == null)
        //    {
        //        throw new Exception($"Роль с именем {roleName} не найдена.");
        //    }

        //    return role.Id;
        //}
    }
}
