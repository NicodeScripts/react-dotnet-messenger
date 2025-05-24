using OptimConnect.Chat.Users;

namespace OptimConnect.Chat.Chatroom
{
    public class UserConversation
    {
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } 
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }
        public required string Title { get; set; }
        public int Id { get; set; } 
    }
}