namespace Artbuk.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }

        public Post() : this(string.Empty) { }

        public Post(string body)
        {
            Body = body;
            CreatedDate = DateTime.Now;
        }
    }
}
