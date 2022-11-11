using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface ILikeRepository
    {
        public Like GetById(Guid likeId);
        public Like CheckIsLiked(Guid postId, Guid userId);
        public void Add(Like like);
        public void Delete(Like like);
        public List<Like> GetListByUserId(Guid userId);
    }
}
