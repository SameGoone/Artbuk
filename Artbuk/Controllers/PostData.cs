using Artbuk.Core.Interfaces;
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

        public PostData(ILikeRepository likeRepository, Post post)
        {
            Post = post;
            LikesCount = likeRepository.GetPostLikesCount(post.Id);
        }
        public PostData(ILikeRepository likeRepository, IPostRepository postRepository, 
            IPostInGenreRepository postInGenreRepository, 
            IGenreRepository genreRepository, IPostInSoftwareRepository postInSoftwareRepository, 
            ISoftwareRepository softwareRepository, Guid postId)
        {
            var postInGenre = postInGenreRepository.GetPostInGenreByPostId(postId);
            var postInSoftware = postInSoftwareRepository.GetPostInSoftwareByPostId(postId);
            Software = softwareRepository.GetById(postInSoftware.SoftwareId);
            Genre = genreRepository.GetById(postInGenre.GenreId);
            Post = postRepository.GetById(postId);
            LikesCount = likeRepository.GetPostLikesCount(postId);
        }
    }
}
