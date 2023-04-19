using Artbuk.Models;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Infrastructure
{
    public class SubscriptionRepository
    {
        private readonly ArtbukContext _dbContext;

        public SubscriptionRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CheckIsSubrcribedTo(Guid subscriberId, Guid followedId)
        {
            return _dbContext.Subscriptions
                .FirstOrDefault(s => s.SubcriberId == subscriberId && s.FollowedId == followedId) != null;
        }

        public Guid Add(Subscription subscription)
        {
            _dbContext.Subscriptions.Add(subscription);
            _dbContext.SaveChanges();

            return subscription.Id;
        }

        public Subscription GetBySubscriberAndFollowed(Guid subscriberId, Guid followedId)
        {
            return _dbContext.Subscriptions
                .FirstOrDefault(s => s.SubcriberId == subscriberId && s.FollowedId == followedId);
        }

        public List<Guid> GetFollowedIds(Guid subscriberId)
        {
            return _dbContext.Subscriptions
                .Where(s => s.SubcriberId == subscriberId && s.FollowedId != null)
                .Select(s => s.FollowedId.Value)
                .ToList();
        }

        public int Remove(Subscription subscription)
        {
            _dbContext.Subscriptions.Remove(subscription);
            return _dbContext.SaveChanges();
        }
    }
}
