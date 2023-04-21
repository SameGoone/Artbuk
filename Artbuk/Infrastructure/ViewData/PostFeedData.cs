using Artbuk.Infrastructure;
using Artbuk.Models;

namespace Artbuk.Infrastructure.ViewData
{
    public class PostFeedData
    {
        public Guid PostId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImagePath { get; set; }


        public PostFeedData(Post post, ImageInPostRepository imageInPostRepository)
        {
            PostId = post.Id;
            CreatedOn = post.CreatedOn;
            ImagePath = Tools.GetImagePath(post.Id, imageInPostRepository);
        }

        public static List<PostFeedData> GetDataRange(List<Post> posts, ImageInPostRepository imageInPostRepository)
        {
            return posts
                .Select(p => new PostFeedData(p, imageInPostRepository))
                .ToList();
        }
    }
}
