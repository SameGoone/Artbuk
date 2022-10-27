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

        public Post GetById(Guid? id)
        {
            return _dbContext.Posts
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Post> List()
        {
            return _dbContext.Posts
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

        public int GetLikesCount(Guid postId)
        {
            return _dbContext.Likes
                .Where(i => i.PostId == postId)
                .Count();
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
