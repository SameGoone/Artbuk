using Artbuk.Infrastructure;
using Artbuk.Models;

namespace Artbuk.Infrastructure.ViewData
{
    public class FeedData
    {
        public List<Genre> Genres { get; set; }
        public List<FeedType> FeedTypes { get; set; }
        public List<Software> Software { get; set; }
        public List<PostFeedData> Posts { get; set; }
        public Guid CurrentUserId { get; set; }
        public FeedOptions Options { get; set; }

        public FeedData(
            List<Genre> genres,
            List<FeedType> feedTypes,
            List<Post> posts,
            List<Software> softwares,
            Guid userId,
            ImageInPostRepository imageInPostRepository,
            FeedOptions options = null)
        {
            Genres = genres;
            FeedTypes = feedTypes;
            Software = softwares;
            CurrentUserId = userId;
            Posts = posts
                .Select(p => new PostFeedData(p, imageInPostRepository))
                .ToList();

            options ??= new FeedOptions();
            Options = options;
        }
    }

    public class FeedOptions
    {
        public Guid? GenreId { get; set; }
        public Guid? FeedTypeId { get; set; }
    }
}
