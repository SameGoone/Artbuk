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
                return Guid.Empty;
            }

            return userEntity.Id;
        }

        public static string SaveImage(IFormFile formFile, Guid? userId, Guid postId)
        {
            var dirPath = $@"wwwroot/images/{userId}";

            var dirInfo = new DirectoryInfo(dirPath);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            var path = $@"{postId}";

            if (!Directory.Exists($"{dirPath}/{path}"))
            {
                dirInfo.CreateSubdirectory(path);
            }

            dirPath += $"/{path}/{Guid.NewGuid()}.{formFile.ContentType.Split('/')[1]}";

            using (var stream = new FileStream(dirPath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }

            return dirPath.Remove(0, 7);
        }

        public static void DeleteImage(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            File.Delete("wwwroot"+path);
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
