namespace Artbuk.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public User? User { get; set; }
        public Guid? UserId { get; set; }

        public Post() : this(string.Empty) { }

        public Post(string body)
        {
            Body = body;
            CreatedOn = DateTime.Now;
        }
    }
}
