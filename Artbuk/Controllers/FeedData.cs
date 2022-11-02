using Artbuk.Core.Interfaces;
using Artbuk.Models;

namespace Artbuk.Controllers
{
    public class FeedData
    {
        public List<Genre> Genres { get; set; }
        public List<Software> Software { get; set; }
        public List<PostData> PostDatas { get; set; }

        public FeedData(IPostRepository postRepository, List<Genre> genres, List<Post> posts, List<Software> softwares)
        {
            Genres = genres;
            Software = softwares;
            PostDatas = posts
                .Select(i => new PostData(postRepository, i))
                .ToList();
        }
    }
}
