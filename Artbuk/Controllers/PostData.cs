using Artbuk.Core.Interfaces;
using Artbuk.Infrastructure;
using Artbuk.Models;

namespace Artbuk.Controllers
{
    public class PostData
    {
        public Post Post { get; set; }

        public int LikesCount { get; set; }

        public PostData(IPostRepository postRepository, Post post)
        {
            Post = post;
            LikesCount = postRepository.GetLikesCountAsync(post.Id).Result;
        }
    }
}
