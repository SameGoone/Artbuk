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

        public bool CheckIsSubrcribedTo(Guid subscribedById, Guid subscribedToId)
        {
            return _dbContext.Subscriptions
                .FirstOrDefault(s => s.SubcribedById == subscribedById && s.SubcribedToId == subscribedToId) != null;
        }

        public Guid Add(Subscription subscription)
        {
            _dbContext.Subscriptions.Add(subscription);
            _dbContext.SaveChanges();

            return subscription.Id;
        }

        public Subscription? GetBySubscribePair(Guid subscribedById, Guid subscribedToId)
        {
            return _dbContext.Subscriptions
                .FirstOrDefault(s => s.SubcribedById == subscribedById && s.SubcribedToId == subscribedToId);
        }

        public List<Guid> GetSubcribedToIds(Guid subscribedById)
        {
            return _dbContext.Subscriptions
                .Where(s => s.SubcribedById == subscribedById && s.SubcribedToId != null)
                .Select(s => s.SubcribedToId.Value)
                .ToList();
        }

        public int Remove(Subscription subscription)
        {
            _dbContext.Subscriptions.Remove(subscription);
            return _dbContext.SaveChanges();
        }
    }
}
