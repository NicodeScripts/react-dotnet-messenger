
using OptimConnect.Chat.Chatroom;

namespace OptimConnect.Chat.Models
{
    public class SaveChatModel
    {
        public string ChatRoomName { get; set; }
        public List<ChatMessage> Messages { get; set; }

        public SaveChatModel()
        {
            ChatRoomName = string.Empty;
            Messages = new List<ChatMessage>();
        }
    }
}