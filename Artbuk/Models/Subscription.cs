namespace Artbuk.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public User? SubcribedBy { get; set; }
        public Guid? SubcribedById { get; set; }
        public User? SubcribedTo { get; set; }
        public Guid? SubcribedToId { get; set; }
    }
}
