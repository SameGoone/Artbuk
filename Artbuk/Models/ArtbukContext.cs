using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace Artbuk.Models
{
    public class ArtbukContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public ArtbukContext(DbContextOptions<ArtbukContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
