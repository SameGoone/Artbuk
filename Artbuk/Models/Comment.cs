namespace Artbuk.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}
