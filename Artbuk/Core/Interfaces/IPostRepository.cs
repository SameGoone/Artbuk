using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> GetByIdAsync(Guid? id);
        Task<List<Post>> GetByIdsAsync(List<Guid> ids);
        Task<List<Post>> ListAsync();
        Task AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(Post post);
        Task DeleteAsync(Guid postId);
        Task<int> GetLikesCountAsync(Guid postId);
    }
}
