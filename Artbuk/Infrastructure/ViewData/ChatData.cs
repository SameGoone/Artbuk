namespace Artbuk.Infrastructure.ViewData
{
    public class ChatData
    {
        public Guid WithUserId { get; set; }
        public string WithUserName { get; set; }
        public string MyName { get; set; }
        public List<ChatMessageData> Messages { get; set; }

        public ChatData(Guid currentUserId,
            Guid withUserId,
            ChatMessageRepository chatMessageRepository,
            UserRepository userRepository)
        {
            var withUser = userRepository.GetById(withUserId);
            var currentUser = userRepository.GetById(currentUserId);
            var messages = chatMessageRepository.GetMessagesByUserIdPair(currentUserId, withUserId);
            var messagesData = messages.Select(m => new ChatMessageData
            {
                Message = m,
                FromMe = m.FromUserId == currentUserId
            })
                .ToList();

            Messages = messagesData;
            WithUserName = withUser.Name;
            WithUserId = withUserId;
            MyName = currentUser.Name + " (Вы)";
        }
    }
}
