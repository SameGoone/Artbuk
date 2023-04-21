using Artbuk.Models;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Infrastructure
{
    public class ArtbukContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ImageInPost> ImageInPosts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<PostInGenre> PostInGenres { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PostInSoftware> PostInSoftware { get; set; }
        public DbSet<Software> Software { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public ArtbukContext(DbContextOptions<ArtbukContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
