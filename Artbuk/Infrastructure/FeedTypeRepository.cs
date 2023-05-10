using Artbuk.Models;
using System.Xml.Linq;

namespace Artbuk.Infrastructure
{
    public class FeedTypeRepository
    {
        private readonly ArtbukContext _dbContext;

        public FeedTypeRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid? GetTypeIdByName(string name)
        {
            return _dbContext.FeedTypes
                .FirstOrDefault(i => i.Name == name)
                ?.Id;
        }

        public FeedType? GetById(Guid typeId)
        {
            return _dbContext.FeedTypes
                .FirstOrDefault(i => i.Id == typeId);
        }

        public List<FeedType> GetAll()
        {
            return _dbContext.FeedTypes
                .OrderBy(i => i.Order)
                .ToList();
        }

        public bool IsTypeGlobal(Guid typeId)
        {
            return GetById(typeId)
                ?.Name == Constants.FeedTypes.Global;
        }

        public bool IsTypeSubscriptionsOnly(Guid typeId)
        {
            return GetById(typeId)
                ?.Name == Constants.FeedTypes.SubscriptionsOnly;
        }

        public bool IsTypeLiked(Guid typeId)
        {
            return GetById(typeId)
                ?.Name == Constants.FeedTypes.Liked;
        }

        public Guid GetGlobalTypeId()
        {
            return GetTypeIdByName(Constants.FeedTypes.Global).Value;
        }

        public Guid GetSubscriptionsOnlyTypeId()
        {
            return GetTypeIdByName(Constants.FeedTypes.SubscriptionsOnly).Value;
        }

        public Guid GetLikedTypeId()
        {
            return GetTypeIdByName(Constants.FeedTypes.Liked).Value;
        }
    }
}
