using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class SubscriptionRepository
    {
        public bool CheckIsSubrcribedTo(Guid subscriberId, Guid followedId)
        {
            throw new NotImplementedException();
        }

        public void Add(Guid subscriberId, Guid followedId)
        {
            throw new NotImplementedException();
        }

        public Subscription GetBySubscriberAndFollowed(Guid subscriberId, Guid followedId)
        {
            throw new NotImplementedException();
        }

        public List<Guid> GetFollowedIds(Guid subscriberId)
        {
            throw new NotImplementedException();
        }

        public int Remove(Subscription subscription)
        {
            throw new NotImplementedException();
        }
    }
}
