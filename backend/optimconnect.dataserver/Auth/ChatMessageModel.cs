namespace OptimConnect.Chat.Auth
{
    public class ChatMessageModel
    {
        public string Message { get; set; }
        public string UserName { get; set; }

        public ChatMessageModel()
        {
            Message = string.Empty;
            UserName = string.Empty;
        }
    }
}