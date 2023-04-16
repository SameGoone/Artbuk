using Artbuk.Infrastructure;
using Artbuk.Models;

namespace Artbuk.Controllers
{
    public class PostFeedData
    {
        public Post Post { get; set; }

        public int LikesCount { get; set; }

        public bool IsLiked { get; set; }

        public ImageInPost ImageInPost { get; set; }

        public PostFeedData(LikeRepository likeRepository, Post post, Guid userId, ImageInPostRepository imageInPostRepository)
        {
            Post = post;
            LikesCount = likeRepository.GetPostLikesCount(post.Id);
            IsLiked = likeRepository.CheckIsPostLikedByUser(post.Id, userId);
            ImageInPost = imageInPostRepository.GetByPostId(post.Id);
        }
    }
}
