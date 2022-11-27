using Artbuk.Core.Interfaces;
using Artbuk.Infrastructure;
using Artbuk.Models;

namespace Artbuk.Controllers
{
    public class PostData
    {
        public Post Post { get; set; }

        public int LikesCount { get; set; }

        public PostData(ILikeRepository likeRepository, Post post)
        {
            Post = post;
            LikesCount = likeRepository.GetPostLikesCount(post.Id);
        }
    }
}
