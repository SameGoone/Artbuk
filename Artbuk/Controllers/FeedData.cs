using Artbuk.Core.Interfaces;
using Artbuk.Models;

namespace Artbuk.Controllers
{
    public class FeedData
    {
        public List<Genre> Genres { get; set; }
        public List<PostData> PostDatas { get; set; }

        public FeedData(IPostRepository postRepository, List<Genre> genres, List<Post> posts)
        {
            Genres = genres;
            PostDatas = posts
                .Select(i => new PostData(postRepository, i))
                .ToList();
        }
    }
}
