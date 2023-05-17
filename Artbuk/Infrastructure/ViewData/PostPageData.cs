using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.Extensions.Hosting;
using System;

namespace Artbuk.Infrastructure.ViewData
{
    public class PostPageData
    {
        public Post Post { get; set; }

        public User Creator { get; set; }

        public string Genre { get; set; }

        public string Software { get; set; }

        public bool IsLiked { get; set; }

        public int LikesCount { get; set; }

        public List<CommentData> Comments { get; set; }

        public string ImagePath { get; set; }

        public bool IsRemovable { get; set; }

        public PostPageData(
            Guid postId, 
            Guid currentUserId, 
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
            var currentUser = userRepository.GetById(currentUserId);

            Post = postRepository.GetById(postId);
            Creator = userRepository.GetById(Post.UserId.Value);
            Software = Tools.GetSoftwareName(postId, softwareRepository, postInSoftwareRepository);
            Genre = Tools.GetGenreName(postId, genreRepository, postInGenreRepository);
            LikesCount = likeRepository.GetPostLikesCount(postId);
            IsLiked = likeRepository.CheckIsPostLikedByUser(postId, currentUserId);

            Comments = GetCommentsData(postId, currentUser, commentRepository, userRepository, roleRepository);

            ImagePath = Tools.GetImagePath(postId, imageInPostRepository);
            IsRemovable = IsRemovable = currentUser.RoleId == roleRepository.GetRoleIdByName("Admin")
                ? true
                : Post.UserId == currentUserId;
        }

        public static List<CommentData> GetCommentsData(Guid postId, User currentUser, CommentRepository commentRepository, UserRepository userRepository, RoleRepository roleRepository)
        {
            return commentRepository.GetCommentsByPostId(postId)
                .Select(comment => new CommentData
                {
                    Id = comment.Id,
                    Body = comment.Body,
                    Creator = userRepository.GetById(comment.UserId),
                    IsRemovable = currentUser.RoleId == roleRepository.GetRoleIdByName("Admin")
                        ? true
                        : comment.UserId == currentUser.Id
                })
                .ToList();
        }
    }
}
