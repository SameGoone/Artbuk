using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class ImageInPostRepository
    {
        private readonly ArtbukContext _dbContext;

        public ImageInPostRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Add(ImageInPost imageInPost)
        {
            _dbContext.ImageInPosts.Add(imageInPost);
            _dbContext.SaveChanges();

            return imageInPost.Id;
        }

        public ImageInPost GetByPostId(Guid postId)
        {
            return _dbContext.ImageInPosts
                .FirstOrDefault(x => x.PostId == postId);
        }

        public List<ImageInPost> GetByPostIds(List<Post> posts)
        {
            var postsIds = posts.Select(p => p.Id).ToList();

            return _dbContext.ImageInPosts
                .Where(i => postsIds.Contains(i.PostId))
                .ToList();
        }

        public int Remove(ImageInPost imageInPost)
        {
            Tools.DeleteImage(imageInPost.ImagePath);
            _dbContext.ImageInPosts.Remove(imageInPost);
            return _dbContext.SaveChanges();
        }

        public void RemoveByPosts(List<Post> posts)
        {
            var imageInPosts = GetByPostIds(posts);

            _dbContext.ImageInPosts.RemoveRange(imageInPosts);
            _dbContext.SaveChanges();
        }
    }
}
