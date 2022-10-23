namespace Artbuk.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public User? Subcriber { get; set; }
        public Guid? SubcriberId { get; set; }
        public User? Followed { get; set; }
        public Guid? FollowedId { get; set; }
    }
}
