using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface IPostInGenreRepository
    {
        List<Guid> GetPostIdsByGenreId(Guid genreId);
        void Add(PostInGenre postInGenre);
    }
}
