using Artbuk.Models;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Infrastructure
{
    public class PostInGenreRepository
    {
        private readonly ArtbukContext _dbContext;

        public PostInGenreRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Add(PostInGenre postInGenre)
        {
            _dbContext.PostInGenres.Add(postInGenre);
            _dbContext.SaveChanges();

            return postInGenre.Id;
        }

        public List<Guid> GetPostIdsByGenreId(Guid genreId)
        {
            return _dbContext.PostInGenres
                .Where(i => i.GenreId == genreId)
                .Select(i => i.PostId)
                .Distinct()
                .ToList();
        }

        public PostInGenre? GetPostInGenreByPostId(Guid postId)
        {
            return _dbContext.PostInGenres
                .FirstOrDefault(i => i.PostId == postId);
        }

        public void Update(PostInGenre postInGenre)
        {
            _dbContext.Entry(postInGenre).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
