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

        public Post GetById(Guid? id)
        {
            return _dbContext.Posts
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Post> GetAll()
        {
            return _dbContext.Posts
                .OrderByDescending(i => i.CreatedDate)
                .ToList();
        }

        public List<Post> GetPostsByUserId(Guid userId)
        {
            return _dbContext.Posts
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.CreatedDate)
                .ToList();
        }

        public void Add(Post post)
        {
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();
        }

        public void Update(Post post)
        {
            _dbContext.Entry(post).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public List<Post> GetByIds(List<Guid> ids)
        {
            return _dbContext.Posts
                .Where(i => ids.Contains(i.Id))
                .ToList();
        }

        public void Delete(Guid postId)
        {
            throw new NotImplementedException();
        }

        public void Delete(Post post)
        {
            _dbContext.Posts.Remove(post);
            _dbContext.SaveChanges();
        }
    }
}
