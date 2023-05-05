using Artbuk.Models;

namespace Artbuk.Infrastructure.ViewData
{
    public class CommentData
    {
        public Guid Id { get; set; }

        public string Body { get; set; }

        public User Creator { get; set; }

        public bool IsRemovable { get; set; }
    }
}
