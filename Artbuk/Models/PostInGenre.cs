namespace Artbuk.Models
{
    public class PostInGenre
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid GenreId { get; set; }
    }
}
