using Artbuk.Models;

namespace Artbuk.Infrastructure.ViewData
{
    public class ChatMessageData
    {
        public bool FromMe { get; set; }
        public ChatMessage Message { get; set; }
    }
}
