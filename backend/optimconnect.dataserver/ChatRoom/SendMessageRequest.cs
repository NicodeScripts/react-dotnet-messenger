namespace OptimConnect.Chat.DTOs
{
    public class SendMessageRequest
    {
        public string Message { get; set; } = string.Empty; // The message content
        public int ConversationId { get; set; }             // The ID of the conversation
        public string SenderId { get; set; } = string.Empty; // The sender's user ID
    }
}
