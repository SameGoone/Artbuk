using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.VisualBasic;
using static Artbuk.Constants;

namespace Artbuk
{
    public static class SampleData
    {
        public static void Initialize(ArtbukContext context, RoleRepository roleRepository)
        {
            if (!context.Genres.Any())
            {
                context.Genres.AddRange
                (
                    new Genre { Name = "Животные" },
                    new Genre { Name = "Архитектура" },
                    new Genre { Name = "Автомобили" }
                );
                context.SaveChanges();
            }

            if (!context.Software.Any())
            {
                context.Software.AddRange
                (
                    new Software { Name = "3dMax" },
                    new Software { Name = "Photoshop" },
                    new Software { Name = "Paint" }
                );
                context.SaveChanges();
            }

            if (!context.Roles.Any())
            {
                context.Roles.AddRange
                (
                    new Role { Name = RoleNames.Admin },
                    new Role { Name = RoleNames.User }
                );
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var userRoleId = Tools.GetRoleId(roleRepository, RoleNames.User);
                context.Users.AddRange
                (
                    new User { Name = "Вадим", Password = "Вадим", RoleId = userRoleId },
                    new User { Name = "Никита", Password = "Никита", RoleId = userRoleId }
                );
                context.SaveChanges();
            }
        }
    }
}
