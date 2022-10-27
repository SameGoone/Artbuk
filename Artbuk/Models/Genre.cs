namespace Artbuk.Models
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class FeedData
    {
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
