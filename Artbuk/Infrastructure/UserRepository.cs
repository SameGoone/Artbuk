using Artbuk.Models;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Infrastructure
{
    public class UserRepository
    {
        private readonly ArtbukContext _dbContext;

        public UserRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetById(Guid id)
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

        public User GetByLogin(string login)
        {
            return _dbContext.Users
                .FirstOrDefault(i => i.Login == login);
        }

        public bool CheckUserExistsWithLogin(string login)
        {
            return _dbContext.Users
                .Any(u => u.Login == login);
        }

        public bool CheckUserExistsWithEmail(string email)
        {
            return _dbContext.Users
                .Any(u => u.Email == email);
        }

        public User GetByCredentials(string login, string password)
        {
            return _dbContext.Users
                .FirstOrDefault(u => u.Login == login && u.Password == password);
        }
    }
}
