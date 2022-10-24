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

                context.SaveChanges();
            }
        }
    }
}
