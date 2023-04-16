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

        public void Add(ImageInPost imageInPost)
        {
            _dbContext.ImageInPosts.Add(imageInPost);
            _dbContext.SaveChanges();
        }

        public ImageInPost GetByPostId(Guid? postId)
        {
            return _dbContext.ImageInPosts
                .FirstOrDefault(x => x.PostId == postId);
        }

        public void Delete(ImageInPost imageInPost)
        {
            Tools.DeleteImage(imageInPost.ImagePath);
            _dbContext.ImageInPosts.Remove(imageInPost);
            _dbContext.SaveChanges();
        }
    }
}
