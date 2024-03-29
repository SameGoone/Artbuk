﻿using Artbuk.Models;
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

        public Genre? GetById(Guid id)
        {
            return _dbContext.Genres
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Genre> GetAll()
        {
            return _dbContext.Genres
                .ToList();
        }

        public Guid Add(Genre genre)
        {
            _dbContext.Genres.Add(genre);
            _dbContext.SaveChanges();

            return genre.Id;
        }

        public Guid Update(Genre genre)
        {
            _dbContext.Entry(genre).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return genre.Id;
        }

        public int Remove(Genre genre)
        {
            _dbContext.Remove(genre);
            return _dbContext.SaveChanges();
        }
    }
}
