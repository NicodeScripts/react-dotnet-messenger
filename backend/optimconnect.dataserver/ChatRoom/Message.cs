using System;
using System.Collections.Generic;
using OptimConnect.Chat.Users;
using OptimConnect.Chat.Chatroom;

namespace OptimConnect.Chat.Chatroom
{
    public class Message
    {
        public int Id { get; set; }
        public required string Owner { get; set; }
        public DateTime sentAt { get; set; }
    }
}