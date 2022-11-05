﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Artbuk.Core.Interfaces;
using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class EfUserRepository : IUserRepository
    {
        private readonly ArtbukContext _dbContext;

        public EfUserRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetById(Guid? id)
        {
            return _dbContext.Users
                .FirstOrDefault(i => i.Id == id);
        }

        public void Add(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public void Update(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public User GetByLogin(string? login)
        {
            return _dbContext.Users
                .FirstOrDefault(i => i.Login == login);
        }

        public User CheckUserLogin(User user)
        {
            return _dbContext.Users
                .FirstOrDefault(u => u.Login == user.Login);
        }

        public User CheckUserEmail(User user)
        {
            return _dbContext.Users
                .FirstOrDefault(u => u.Email == user.Email);
        }

        public User CheckUserExistence(string login, string password)
        {
            return _dbContext.Users
                .FirstOrDefault(u => u.Login == login && u.Password == password);
        }
    }
}
