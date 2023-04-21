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

        public int Remove(ImageInPost imageInPost)
        {
            Tools.DeleteImage(imageInPost.ImagePath);
            _dbContext.ImageInPosts.Remove(imageInPost);
            return _dbContext.SaveChanges();
        }
    }
}
