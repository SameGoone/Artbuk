using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.Extensions.Hosting;
using System;

namespace Artbuk.Infrastructure.ViewData
{
    public class PostPageData
    {
        public Post Post { get; set; }

        public User User { get; set; }

        public string Genre { get; set; }

        public string Software { get; set; }

        public bool IsLiked { get; set; }

        public int LikesCount { get; set; }

        public List<CommentData> Comments { get; set; }

        public string ImagePath { get; set; }

        public bool IsRemovable { get; set; }

        public PostPageData(
            Guid postId, 
            Guid userId, 
            LikeRepository likeRepository, 
            PostRepository postRepository,
            PostInGenreRepository postInGenreRepository,
            GenreRepository genreRepository, 
            PostInSoftwareRepository postInSoftwareRepository,
            SoftwareRepository softwareRepository, 
            CommentRepository commentRepository, 
            ImageInPostRepository imageInPostRepository,
            RoleRepository roleRepository,
            UserRepository userRepository)
        {
            var currentUser = userRepository.GetById(userId);

            User = currentUser;
            Software = Tools.GetSoftwareName(postId, softwareRepository, postInSoftwareRepository);
            Genre = Tools.GetGenreName(postId, genreRepository, postInGenreRepository);
            Post = postRepository.GetById(postId);
            LikesCount = likeRepository.GetPostLikesCount(postId);
            IsLiked = likeRepository.CheckIsPostLikedByUser(postId, userId);


            Comments = commentRepository.GetCommentsByPostId(postId)
                .Select(comment => new CommentData
                {
                    Id = comment.Id,
                    Body = comment.Body,
                    User = currentUser,
                    IsRemovable = currentUser.RoleId == roleRepository.GetRoleIdByName("Admin")
                        ? true
                        : comment.UserId == userId
                })
                .ToList();

            ImagePath = Tools.GetImagePath(postId, imageInPostRepository);
            IsRemovable = Post.UserId == userId;
        }
    }
}
