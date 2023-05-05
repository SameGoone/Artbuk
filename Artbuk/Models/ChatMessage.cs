namespace Artbuk.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Body { get; set; }
        public Guid? FromUserId { get; set; }
        public User? FromUser { get; set; }
        public Guid? ToUserId { get; set; }
        public User? ToUser { get; set; }
    }
}
