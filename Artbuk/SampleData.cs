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
            if (!context.FeedTypes.Any())
            {
                context.FeedTypes.AddRange
                (
                    new FeedType { Name = FeedTypes.Global, Order = 0 },
                    new FeedType { Name = FeedTypes.SubscriptionsOnly, Order = 1 },
                    new FeedType { Name = FeedTypes.Liked, Order = 2 }
                );
                context.SaveChanges();
            }

            if (!context.Genres.Any())
            {
                context.Genres.AddRange
                (
                    new Genre { Name = "Животные" },
                    new Genre { Name = "Архитектура" },
                    new Genre { Name = "Автомобили" },
                    new Genre { Name = "Комикс" },
                    new Genre { Name = "Спорт" },
                    new Genre { Name = "Ретро" },
                    new Genre { Name = "Плакат" },
                    new Genre { Name = "Фэшн" }
                );
                context.SaveChanges();
            }

            if (!context.Software.Any())
            {
                context.Software.AddRange
                (
                    new Software { Name = "Corel Painter" },
                    new Software { Name = "Adobe Photoshop" },
                    new Software { Name = "Adobe Illustrator" },
                    new Software { Name = "Paint" },
                    new Software { Name = "3D Max" },
                    new Software { Name = "Maya" },
                    new Software { Name = "ZBrush" },
                    new Software { Name = "GIMP" }
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
                var adminRoleId = Tools.GetRoleId(roleRepository, RoleNames.Admin);
                context.Users.AddRange
                (
                    new User { Name = "Вадим", Password = Tools.HashPassword("Вадим"), Email = "vadim@mail.ru", RoleId = userRoleId },
                    new User { Name = "Никита", Password = Tools.HashPassword("Никита"), Email = "nikita@mail.ru", RoleId = userRoleId },
                    new User { Name = "Соня", Password = Tools.HashPassword("Соня"), Email = "sonya@mail.ru", RoleId = userRoleId },
                    new User { Name = "Евгений", Password = Tools.HashPassword("Евгений"), Email = "zheka@mail.ru", RoleId = userRoleId },
                    new User { Name = "Админ", Password = Tools.HashPassword("Админ"), Email = "admin@mail.ru", RoleId = adminRoleId }
                );
                context.SaveChanges();
            }
        }
    }
}
