using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface IPostInGenreRepository
    {
        Task<List<Guid>> GetPostIdsByGenreIdAsync(Guid genreId);
        Task AddAsync(PostInGenre postInGenre);
    }
}
