using OptimConnect.Chat.Users;
namespace OptimConnect.Chat.Chatroom
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty; // Content of the message
        public DateTime Timestamp { get; set; } // When the message was sent
        public string SenderId { get; set; } = string.Empty;// Sender's user ID (Foreign Key)
        public User Sender { get; set; } // Navigation property for the sender
        public int ConversationId { get; set; } // The conversation this message belongs to
        public Conversation Conversation { get; set; }// Navigation property for the conversation
    }

}
// using OptimConnect.Chat.Users;

// namespace OptimConnect.Chat.Chatroom
// {
//     public class ChatMessage
//     {
//         public int Id { get; set; }
//         public string Content { get; set; } = string.Empty;
//         public DateTime SentAt { get; set; } = DateTime.UtcNow;
//         public string SenderId { get; set; } = string.Empty;
//         public User Sender { get; set; } = null!; // Navigation property
//         public int ConversationId { get; set; }
//         public Conversation Conversation { get; set; } = null!; // Navigation property

//         public ChatMessage() { }

//         public ChatMessage(string content, string senderId, int conversationId)
//         {
//             Content = content;
//             SenderId = senderId;
//             ConversationId = conversationId;
//             SentAt = DateTime.UtcNow;
//         }
//     }
// }
