using Artbuk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Artbuk.Infrastructure
{
    public class ChatMessageRepository
    {
        private readonly ArtbukContext _dbContext;

        public ChatMessageRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ChatMessage? GetById(Guid id)
        {
            return _dbContext.ChatMessages
                .FirstOrDefault(i => i.Id == id);
        }

        public Guid Add(ChatMessage message)
        {
            _dbContext.ChatMessages.Add(message);
            _dbContext.SaveChanges();

            return message.Id;
        }

        public List<ChatMessage> GetMessagesByUserIdPair(Guid userId1, Guid userId2)
        {
            return _dbContext.ChatMessages
                .Include(m => m.FromUser)
                .Where(m => m.FromUserId == userId1 && m.ToUserId == userId2 ||
                       m.FromUserId == userId2 && m.ToUserId == userId1)
                .OrderBy(m => m.CreatedOn)
                .ToList();
        }

        public int RemoveMessagesByUserId(Guid userId)
        {
            var messages = GetMessagesByUserId(userId);
            _dbContext.ChatMessages.RemoveRange(messages);

            return _dbContext.SaveChanges();
        }

        private List<ChatMessage> GetMessagesByUserId(Guid userId)
        {
            return _dbContext.ChatMessages
                .Where(m => m.FromUserId == userId || m.ToUserId == userId)
                .ToList();
        }

        public int Remove(ChatMessage message)
        {
            _dbContext.ChatMessages.Remove(message);
            return _dbContext.SaveChanges();
        }
    }
}
