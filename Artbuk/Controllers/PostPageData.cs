using Artbuk.Infrastructure;
using Artbuk.Models;
using System;

namespace Artbuk.Controllers
{
    public class PostPageData
    {
        public Post Post { get; set; }

        public Genre Genre { get; set; }

        public Software Software { get; set; }

        public bool IsLiked { get; set; }

        public int LikesCount { get; set; }

        public List<Comment> Comments { get; set; }

        public ImageInPost ImageInPost { get; set; }

        public PostPageData(Guid postId, Guid userId, LikeRepository likeRepository, PostRepository postRepository,
            PostInGenreRepository postInGenreRepository,
            GenreRepository genreRepository, PostInSoftwareRepository postInSoftwareRepository,
            SoftwareRepository softwareRepository, CommentRepository commentRepository, ImageInPostRepository imageInPostRepository)
        {
            var postInGenre = postInGenreRepository.GetPostInGenreByPostId(postId);
            var postInSoftware = postInSoftwareRepository.GetPostInSoftwareByPostId(postId);
            Software = softwareRepository.GetById(postInSoftware.SoftwareId);
            Genre = genreRepository.GetById(postInGenre.GenreId);
            Post = postRepository.GetById(postId);
            LikesCount = likeRepository.GetPostLikesCount(postId);
            IsLiked = likeRepository.CheckIsPostLikedByUser(postId, userId);
            Comments = commentRepository.GetComments(postId);
            ImageInPost = imageInPostRepository.GetByPostId(postId);
        }
    }
}
