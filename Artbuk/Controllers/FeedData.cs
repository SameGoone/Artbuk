using Artbuk.Infrastructure;
using Artbuk.Models;

namespace Artbuk.Controllers
{
    public class FeedData
    {
        public List<Genre> Genres { get; set; }
        public List<Software> Software { get; set; }
        public List<PostFeedData> Posts { get; set; }
        public Guid CurrentUserId { get; set; }

        public FeedData(
            LikeRepository likeRepository, 
            List<Genre> genres, 
            List<Post> posts, 
            List<Software> softwares, 
            Guid userId, 
            ImageInPostRepository imageInPostRepository)
        {
            Genres = genres;
            Software = softwares;
            CurrentUserId = userId;
            Posts = posts
                .Select(p => new PostFeedData(p, imageInPostRepository))
                .ToList();
        }
    }
}
