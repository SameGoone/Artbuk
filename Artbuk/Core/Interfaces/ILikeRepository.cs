using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface ILikeRepository
    {
        public Like GetById(Guid likeId);
        public bool CheckIsPostLikedByUser(Guid postId, Guid userId);
        public Like GetLikeOnPostByUser(Guid postId, Guid userId);
        public void Add(Like like);
        public void Delete(Like like);
        public List<Like> GetListByUserId(Guid userId);
        int GetPostLikesCount(Guid postId);
    }
}
