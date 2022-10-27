using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface IGenreRepository
    {
        Genre GetById(Guid id);
        List<Genre> List();
        void Add(Genre genre);
        void Update(Genre genre);
    }
}
