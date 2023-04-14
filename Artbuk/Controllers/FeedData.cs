using Artbuk.Infrastructure;
using Artbuk.Models;

namespace Artbuk.Controllers
{
    public class FeedData
    {
        public List<Genre> Genres { get; set; }
        public List<Software> Software { get; set; }
        public List<PostFeedData> PostDatas { get; set; }

        public FeedData(LikeRepository likeRepository, List<Genre> genres, List<Post> posts, List<Software> softwares, Guid userId)
        {
            Genres = genres;
            Software = softwares;
            PostDatas = posts
                .Select(i => new PostFeedData(likeRepository, i, userId))
                .ToList();
        }
    }
}
