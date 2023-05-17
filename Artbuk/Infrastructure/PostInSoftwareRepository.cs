using Artbuk.Models;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Infrastructure
{
    public class PostInSoftwareRepository
    {
        private readonly ArtbukContext _dbContext;
        public PostInSoftwareRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Add(PostInSoftware postInSoftware)
        {
            _dbContext.PostInSoftware.Add(postInSoftware);
            _dbContext.SaveChanges();

            return postInSoftware.Id;
        }

        public List<Guid> GetPostIdsBySoftwareId(Guid softwareId)
        {
            return _dbContext.PostInSoftware
                .Where(i => i.SoftwareId == softwareId)
                .Select(i => i.PostId)
                .Distinct()
                .ToList();
        }

        public PostInSoftware? GetPostInSoftwareByPostId(Guid postId)
        {
            return _dbContext.PostInSoftware
                .FirstOrDefault(i => i.PostId == postId);
        }

        public void Update(PostInSoftware postInSoftware)
        {
            _dbContext.Entry(postInSoftware).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
