using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.Extensions.Hosting;
using System;

namespace Artbuk.Controllers
{
    public class PostPageData
    {
        public Post Post { get; set; }

        public string Genre { get; set; }

        public string Software { get; set; }

        public bool IsLiked { get; set; }

        public int LikesCount { get; set; }

        public List<Comment> Comments { get; set; }

        public string ImagePath { get; set; }

        public PostPageData(Guid postId, Guid userId, LikeRepository likeRepository, PostRepository postRepository,
            PostInGenreRepository postInGenreRepository,
            GenreRepository genreRepository, PostInSoftwareRepository postInSoftwareRepository,
            SoftwareRepository softwareRepository, CommentRepository commentRepository, ImageInPostRepository imageInPostRepository)
        {
            Software = Tools.GetSoftwareName(postId, softwareRepository, postInSoftwareRepository);
            Genre = Tools.GetGenreName(postId, genreRepository, postInGenreRepository);
            Post = postRepository.GetById(postId);
            LikesCount = likeRepository.GetPostLikesCount(postId);
            IsLiked = likeRepository.CheckIsPostLikedByUser(postId, userId);
            Comments = commentRepository.GetComments(postId);
            ImagePath = Tools.GetImagePath(postId, imageInPostRepository);
        }
    }
}
