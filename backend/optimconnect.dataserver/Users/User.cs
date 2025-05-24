// using System.Collections.Generic;
// using OptimConnect.Chat.Chatroom;
// using Microsoft.AspNetCore.Identity;

// namespace OptimConnect.Chat.Users
// {
//     public class User : IdentityUser
//     {
        
//         public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
//         public ICollection<UserConversation> UserConversations { get; set; } = new List<UserConversation>();
//     }
// }


using System.Collections.Generic;
using OptimConnect.Chat.Chatroom;
using Microsoft.AspNetCore.Identity;

namespace OptimConnect.Chat.Users
{
    public class User : IdentityUser
    {
        public override string Id { get; set; }
        public override string UserName { get; set; } = string.Empty;
        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
        public ICollection<UserConversation> UserConversations { get; set; } = new List<UserConversation>();
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>(); // For tracking messages sent by the user
    }
}