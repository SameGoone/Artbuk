using Artbuk.Models;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Infrastructure
{
    public class GenreRepository
    {
        private readonly ArtbukContext _dbContext;

        public GenreRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Genre GetById(Guid id)
        {
            return _dbContext.Genres
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Genre> GetAll()
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
