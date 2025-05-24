using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using OptimConnect.Chat.DataService;
using OptimConnect.Chat.Chatroom; // Ensure this namespace is used
using System.Threading.Tasks;

namespace OptimConnect.Chat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        // private readonly ApplicationDbContext _context;

        // public ChatHub(ApplicationDbContext context)
        // {
        //     _context = context;
        // }

        // public async Task SendMessage(string roomId, string message)
        // {
        //     var userName = Context.User?.Identity?.Name;
        //     if (userName == null)
        //     {
        //         throw new HubException("User not authenticated.");
        //     }

        //     var chatRoom = await _context.ChatRooms.FindAsync(roomId);
        //     if (chatRoom == null)
        //     {
        //         throw new HubException("Chat room not found.");
        //     }

        //     var chatMessage = new ChatMessage
        //     {
        //         Message = message,
        //         UserName = userName,
        //         ChatRoomId = roomId,
        //         ChatRoom = chatRoom
        //     };

        //     _context.ChatMessages.Add(chatMessage);
        //     await _context.SaveChangesAsync();

        //     await Clients.Group(roomId).SendAsync("ReceiveMessage", userName, message);
        // }

        // public async Task JoinRoom(string roomId)
        // {
        //     await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        // }

        // public async Task LeaveRoom(string roomId)
        // {
        //     await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        // }
    }
}