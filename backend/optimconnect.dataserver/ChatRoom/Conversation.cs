using System.Collections.Generic;
using OptimConnect.Chat.Users;

// namespace OptimConnect.Chat.Chatroom
// {
//     public class Conversation
//     {
//         public int Id { get; set; }
//         public string Title { get; set; } = string.Empty;
//         public string CreatedBy { get; set; } = string.Empty;
//         public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//         public ICollection<User> Users { get; set; } = new List<User>();
//         public ICollection<UserConversation> UserConversations { get; set; } = new List<UserConversation>();
//         public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

//         public Conversation() { }

//         public Conversation(string title, string createdBy)
//         {
//             Title = title;
//             CreatedBy = createdBy;
//             CreatedAt = DateTime.UtcNow;
//         }
//     }
// }
namespace OptimConnect.Chat.Chatroom
{
    public class Conversation
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>(); // Messages in the conversation
        public ICollection<UserConversation> UserConversations { get; set; } = new List<UserConversation>();
    }
}