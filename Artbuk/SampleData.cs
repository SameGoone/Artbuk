using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.VisualBasic;

namespace Artbuk
{
    public static class SampleData
    {
        public static void Initialize(ArtbukContext context)
        {
            if (!context.Genres.Any())
            {
                context.Genres.AddRange
                (
                    new Genre { Name = "Животные" },
                    new Genre { Name = "Архитектура" },
                    new Genre { Name = "Автомобили" }
                );

                context.Software.AddRange
                (
                    new Software { Name = "3dMax" },
                    new Software { Name = "Photoshop" },
                    new Software { Name = "Paint" }
                );

                context.Roles.AddRange
                (
                    new Role { Name = "Admin"},
                    new Role { Name = "User" }
                );

                context.SaveChanges();
            }
        }
    }
}
