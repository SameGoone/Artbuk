namespace Artbuk.Models
{
    public class PostInGenre
    {
        public Guid Id { get; set; }
        public Post Post { get; set; }
        public Guid PostId { get; set; }
        public Genre Genre { get; set; }
        public Guid GenreId { get; set; }
    }
}
