using Artbuk.Infrastructure;
using Artbuk.Models;

namespace Artbuk.Controllers
{
    public class PostFeedData
    {
        public Post Post { get; set; }

        public string ImagePath { get; set; }

        public PostFeedData(Post post, ImageInPostRepository imageInPostRepository)
        {
            Post = post;
            ImagePath = Tools.GetImagePath(post.Id, imageInPostRepository);
        }
    }
}
