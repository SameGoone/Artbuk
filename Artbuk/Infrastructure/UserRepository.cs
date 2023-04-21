using Artbuk.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Artbuk.Infrastructure
{
    public class UserRepository
    {
        private readonly ArtbukContext _dbContext;

        public UserRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User? GetById(Guid id)
        {
            return _dbContext.Users
                .FirstOrDefault(i => i.Id == id);
        }

        public List<User> GetByIds(List<Guid> Ids)
        {
            return _dbContext.Users
                .Where(u => Ids.Contains(u.Id))
                .ToList();
        }

        public List<User> GetAll()
        {
            return _dbContext.Users.ToList();
        }

        public Guid Add(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public int Update(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }

        public User? GetByName(string name)
        {
            return _dbContext.Users
                .FirstOrDefault(i => i.Name == name);
        }

        public string GetNameById(Guid userId)
        {
            var user = GetById(userId);
            return user != null
                ? user.Name
                : string.Empty;
        }

        public bool CheckUserExistsWithName(string name)
        {
            return _dbContext.Users
                .Any(u => u.Name == name);
        }

        public bool CheckUserExistsWithEmail(string email)
        {
            return _dbContext.Users
                .Any(u => u.Email == email);
        }

        public User? GetByCredentials(string name, string password)
        {
            return _dbContext.Users
                .FirstOrDefault(u => u.Name == name && u.Password == password);
        }
    }
}
