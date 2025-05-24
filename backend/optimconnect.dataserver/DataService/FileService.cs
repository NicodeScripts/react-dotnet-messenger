using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace OptimConnect.Chat.DataService
{
    public class FileService
    {
        private readonly string _basePath;

        public FileService()
        {
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "ChatRooms");
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task SaveMessagesAsync(string chatRoom, ConcurrentQueue<string> messages)
        {
            string filePath = GetFilePath(chatRoom);
            using (StreamWriter writer = new StreamWriter(filePath, append: false))
            {
                foreach (var message in messages)
                {
                    await writer.WriteLineAsync(message);
                }
            }
        }

        public async Task<ConcurrentQueue<string>> LoadMessagesAsync(string chatRoom)
        {
            string filePath = GetFilePath(chatRoom);
            ConcurrentQueue<string> messages = new ConcurrentQueue<string>();

            if (File.Exists(filePath))
            {
                string[] lines = await File.ReadAllLinesAsync(filePath);
                foreach (string line in lines)
                {
                    messages.Enqueue(line);
                }
            }

            return messages;
        }

        private string GetFilePath(string chatRoom)
        {
            return Path.Combine(_basePath, $"{chatRoom}.txt");
        }
    }
}