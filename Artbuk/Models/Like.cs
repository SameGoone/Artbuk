namespace Artbuk.Models
{
    public class Like
    {
        public Guid Id { get; set; }
        public Post Post { get; set; }
        public Guid PostId { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }

        public Like() : this(Guid.Empty, Guid.Empty) { }

        public Like(Guid postId, Guid userId)
        {
            PostId = postId;
            UserId = userId;
        }
    }
}
