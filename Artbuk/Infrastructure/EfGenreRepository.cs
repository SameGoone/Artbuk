using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Artbuk.Core.Interfaces;
using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class EfGenreRepository : IGenreRepository
    {
        private readonly ArtbukContext _dbContext;

        public EfGenreRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Genre GetById(Guid id)
        {
            return _dbContext.Genres
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Genre> List()
        {
            return _dbContext.Genres
                .ToList();
        }

        public void Add(Genre genre)
        {
            _dbContext.Genres.Add(genre);
            _dbContext.SaveChanges();
        }

        public void Update(Genre genre)
        {
            _dbContext.Entry(genre).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
