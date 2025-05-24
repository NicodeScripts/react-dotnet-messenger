using System.Collections.Concurrent;
using OptimConnect.Chat.Models;

namespace OptimConnect.Chat.DataService
{
    public class SharedDB
    {
        private readonly ConcurrentDictionary<string, UserConnection> _connections = new();

        public ConcurrentDictionary<string, UserConnection> Connections => _connections;
    }
}