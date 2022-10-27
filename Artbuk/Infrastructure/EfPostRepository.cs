using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Artbuk.Core.Interfaces;
using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class EfPostRepository : IPostRepository
    {
        private readonly ArtbukContext _dbContext;

        public EfPostRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Post> GetByIdAsync(Guid? id)
        {
            return _dbContext.Posts
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<List<Post>> ListAsync()
        {
            return _dbContext.Posts
                .OrderByDescending(i => i.CreatedDate)
                .ToListAsync();
        }

        public Task AddAsync(Post post)
        {
            _dbContext.Posts.Add(post);
            return _dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(Post post)
        {
            _dbContext.Entry(post).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }

        public Task<int> GetLikesCountAsync(Guid postId)
        {
            return _dbContext.Likes
                .Where(i => i.PostId == postId)
                .CountAsync();
        }

        public Task<List<Post>> GetByIdsAsync(List<Guid> ids)
        {
            return _dbContext.Posts
                .Where(i => ids.Contains(i.Id))
                .ToListAsync();
        }

        public Task DeleteAsync(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Post post)
        {
            _dbContext.Posts.Remove(post);
            return _dbContext.SaveChangesAsync();
        }
    }
}
