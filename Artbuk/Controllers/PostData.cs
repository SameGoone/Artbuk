using Artbuk.Infrastructure;
using Artbuk.Models;

namespace Artbuk.Controllers
{
    public class PostData
    {
        public Post Post { get; set; }

        public int LikesCount { get; set; }

        public Genre Genre { get; set; }

        public Software Software { get; set; }

        public bool IsLiked { get; set; }

        public PostData(LikeRepository likeRepository, Post post, Guid userId)
        {
            Post = post;
            LikesCount = likeRepository.GetPostLikesCount(post.Id);
            IsLiked = likeRepository.CheckIsPostLikedByUser(post.Id, userId);
        }

        public PostData(LikeRepository likeRepository, PostRepository postRepository,
            PostInGenreRepository postInGenreRepository,
            GenreRepository genreRepository, PostInSoftwareRepository postInSoftwareRepository,
            SoftwareRepository softwareRepository, Guid postId, Guid userId)
        {
            var postInGenre = postInGenreRepository.GetPostInGenreByPostId(postId);
            var postInSoftware = postInSoftwareRepository.GetPostInSoftwareByPostId(postId);
            Software = softwareRepository.GetById(postInSoftware.SoftwareId);
            Genre = genreRepository.GetById(postInGenre.GenreId);
            Post = postRepository.GetById(postId);
            LikesCount = likeRepository.GetPostLikesCount(postId);
            IsLiked = likeRepository.CheckIsPostLikedByUser(postId, userId);
        }
    }
}
