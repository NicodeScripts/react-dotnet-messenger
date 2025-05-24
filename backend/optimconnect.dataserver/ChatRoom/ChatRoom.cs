using System.Collections.Generic;
using OptimConnect.Chat.Users;

namespace OptimConnect.Chat.Chatroom
{
    public class ChatRoom
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }

        public ChatRoom()
        {
            Id = string.Empty;
            Name = string.Empty;
            UserId = string.Empty;
            User = new User();
            ChatMessages = new List<ChatMessage>();
        }

        public class Conversation
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string? CreatedBy { get; set; } 
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            // Navigation properties
            public ICollection<User> Users { get; set; } = new List<User>();

            // Public constructor for EF Core and general usage
            public Conversation(string title, string createdBy)
            {
                Title = title ?? throw new ArgumentNullException(nameof(title));
                CreatedBy = createdBy ?? throw new ArgumentNullException(nameof(createdBy));
            }

            // Public parameterless constructor for EF Core
            public Conversation() { }
        }
    }
}
