using Artbuk.Infrastructure;
using Artbuk.Models;
using System.Security.Claims;

namespace Artbuk
{
    public class Tools
    {
        private const string _defaultImage = "wwwroot/images/defaultImage";
        private const string _defaultName = "null";

        public static Guid GetUserId(UserRepository userRepository, ClaimsPrincipal user)
        {
            var userName = user.Identity.Name;
            var userEntity = userRepository.GetByName(userName);

            if (userEntity == null)
            {
                return Guid.Empty;
            }

            return userEntity.Id;
        }

        public static string SavePostImage(IFormFile? formFile, Guid? userId, Guid? postId)
        {
            var dirPath = $"wwwroot/images/{userId}";

            var dirInfo = new DirectoryInfo(dirPath);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            var path = $"{postId}";

            if (!Directory.Exists($"{dirPath}/{path}"))
            {
                dirInfo.CreateSubdirectory(path);
            }

            var fileType = formFile.ContentType.Split('/')[1];
            var fileName = $"{Guid.NewGuid()}.{fileType}";
            dirPath += $"/{path}/{fileName}";

            using (var stream = new FileStream(dirPath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }

            return dirPath.Replace("wwwroot", "");
        }

        public static string SaveUserImage(IFormFile? formFile, Guid? userId)
        {
            var dirPath = $"wwwroot/images/users";

            var dirInfo = new DirectoryInfo(dirPath);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            var fileType = formFile.ContentType.Split('/')[1];
            var fileName = $"{userId}.{fileType}";
            dirPath += $"/{fileName}";

            using (var stream = new FileStream(dirPath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }

            return dirPath.Replace("wwwroot", "");
        }

        public static void DeleteImage(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            File.Delete("wwwroot" + path);
        }

        public static string GetImagePath(Guid postId, ImageInPostRepository imageInPostRepository)
        {
            var imageInPost = imageInPostRepository.GetByPostId(postId);
            return imageInPost?.ImagePath ?? _defaultImage;
        }

        public static string GetImagePath(string imagePath)
        {
            return imagePath ?? _defaultImage;
        }

        public static string GetSoftwareName(Guid postId, SoftwareRepository softwareRepository, PostInSoftwareRepository postInSoftwareRepository)
        {
            var postInSoftware = postInSoftwareRepository.GetPostInSoftwareByPostId(postId);

            if (postInSoftware == null)
            {
                return _defaultName;
            }

            return softwareRepository.GetById(postInSoftware.SoftwareId)?.Name ?? _defaultName;
        }

        public static string GetGenreName(Guid postId, GenreRepository genreRepository, PostInGenreRepository postInGenreRepository)
        {
            var postInGenre = postInGenreRepository.GetPostInGenreByPostId(postId);

            if (postInGenre == null)
            {
                return _defaultName;
            }

            return genreRepository.GetById(postInGenre.GenreId)?.Name ?? _defaultName;
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
