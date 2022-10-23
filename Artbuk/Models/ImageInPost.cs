namespace Artbuk.Models
{
    public class ImageInPost
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public Post? Post { get; set; }
        public Guid? PostId { get; set; }
    }
}
