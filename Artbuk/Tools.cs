using Artbuk.Infrastructure;
using Artbuk.Models;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Artbuk
{
    public class Tools
    {
        private const string _defaultImage = "wwwroot/images/defaultImage.jpg";
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

        public static Guid GetRoleId(RoleRepository roleRepository, string roleName)
        {
            var roleId = roleRepository.GetRoleIdByName(roleName);
            if (roleId == null)
            {
                throw new Exception($"Роль с именем {roleName} не найдена.");
            }

            return roleId.Value;
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return (buffer3.SequenceEqual(buffer4));
        }
    }
}
