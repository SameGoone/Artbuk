namespace Artbuk.Models
{
    public class PostInSoftware
    {
        public Guid Id { get; set; }
        public Post Post { get; set; }
        public Guid PostId { get; set; }
        public Software Software { get; set; }
        public Guid SoftwareId { get; set; }
    }
}
