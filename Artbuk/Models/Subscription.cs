namespace Artbuk.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public Guid SubcriberId { get; set; }
        public Guid FollowedId { get; set; }
    }
}
