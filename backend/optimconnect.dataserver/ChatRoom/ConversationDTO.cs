namespace OptimConnect.Chat.DTOs
{
    public class ConversationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<string> UserNames { get; set; } = new List<string>();
    }
}