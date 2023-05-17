using Artbuk.Models;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Infrastructure
{
    public class PostRepository
    {
        private readonly ArtbukContext _dbContext;

        public PostRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Post? GetById(Guid id)
        {
            return _dbContext.Posts
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Post> GetAll()
        {
            return _dbContext.Posts
                .OrderByDescending(i => i.CreatedOn)
                .ToList();
        }

        public List<Guid> GetAllIds()
        {
            return _dbContext.Posts
                .OrderByDescending(i => i.CreatedOn)
                .Select(i => i.Id)
                .ToList();
        }

        public List<Post> GetPostsByUserId(Guid userId)
        {
            return _dbContext.Posts
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.CreatedOn)
                .ToList();
        }

        public Guid Add(Post post)
        {
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();

            return post.Id;
        }

        public int Update(Post post)
        {
            _dbContext.Entry(post).State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }

        public List<Post> GetByIds(List<Guid> ids)
        {
            return _dbContext.Posts
                .Where(i => ids.Contains(i.Id))
                .ToList();
        }

        public int Remove(Post post)
        {
            _dbContext.Posts.Remove(post);
            return _dbContext.SaveChanges();
        }

        public int RemovePostsByUserId(Guid userId)
        {
            var posts = GetPostsByUserId(userId);
            _dbContext.Posts.RemoveRange(posts);

            return _dbContext.SaveChanges();
        }

        public List<Post> GetPostsByContentMatch(string searchText)
        {
            return _dbContext.Posts
                .Where(p => p.Body.Contains(searchText))
                .ToList();
        }
    }
}
