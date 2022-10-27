using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface IGenreRepository
    {
        Task<Genre> GetByIdAsync(Guid id);
        Task<List<Genre>> ListAsync();
        Task AddAsync(Genre genre);
        Task UpdateAsync(Genre genre);
    }
}
