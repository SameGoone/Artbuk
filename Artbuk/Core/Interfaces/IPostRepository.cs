using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface IPostRepository
    {
        Post GetById(Guid? id);
        List<Post> GetByIds(List<Guid> ids);
        List<Post> ListAll();
        List<Post> ListByUserId(Guid userId);
        void Add(Post post);
        void Update(Post post);
        void Delete(Post post);
        void Delete(Guid postId);
        int GetLikesCount(Guid postId);
    }
}
