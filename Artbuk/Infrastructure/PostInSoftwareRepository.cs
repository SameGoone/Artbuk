using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class PostInSoftwareRepository
    {
        private readonly ArtbukContext _dbContext;
        public PostInSoftwareRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(PostInSoftware postInSoftware)
        {
            if (postInSoftware != null)
            {
                _dbContext.PostInSoftware.Add(postInSoftware);
                _dbContext.SaveChanges();
            }
        }

        public List<Guid> GetPostIdsBySoftwareId(Guid softwareId)
        {
            return _dbContext.PostInSoftware
                .Where(i => i.SoftwareId == softwareId)
                .Select(i => i.PostId)
                .Distinct()
                .ToList();
        }

        public PostInSoftware GetPostInSoftwareByPostId(Guid postId)
        {
            return _dbContext.PostInSoftware
                .FirstOrDefault(i => i.PostId == postId);
        }
    }
}
