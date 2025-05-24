using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using OptimConnect.Chat.Users;
using OptimConnect.Chat.DataService;
using OptimConnect.Chat.Chatroom;
using OptimConnect.Chat.DTOs;

[ApiController]
[Route("api/[controller]")]
public class ConversationsController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public ConversationsController(UserManager<User> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    // Endpoint to fetch conversations for the current user
    [HttpGet("currentUser")]
    public async Task<IActionResult> GetConversationsForCurrentUser()
    {
        var currentUserName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserName == null)
        {
            return Unauthorized(new { Message = "User not authenticated." });
        }
        var currentUser = await _userManager.FindByNameAsync(currentUserName);
        if (currentUser == null)
        {
            return NotFound(new { Message = "Current user not found." });
        }

        var currentUserId = currentUser.Id;

        var userConversations = await _context.UserConversations
            .Where(uc => uc.UserId == currentUserId)
            .Include(uc => uc.Conversation)
            .ThenInclude(c => c.UserConversations)
            .ThenInclude(uc => uc.User)
            .ToListAsync();

        // Prepare the DTOs
        var conversationDtos = userConversations.Select(uc => new ConversationDto
        {
            Id = uc.Conversation.Id,
            Title = uc.Title,
            CreatedBy = uc.Conversation.CreatedBy,
            CreatedAt = uc.Conversation.CreatedAt,
            UserNames = uc.Conversation.UserConversations
                .Where(participant => participant.UserId != currentUserId)
                .Select(participant => participant.User.UserName)
                .ToList()
        }).ToList();

        return Ok(conversationDtos);
    }

    [HttpPost("newconversation/{otherUserId}")]
    public async Task<IActionResult> CreateOrGetConversation(string otherUserId)
    {

        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserId == null)
        {
            return Unauthorized(new { Message = "User not authenticated." });
        }

        var currentUser = await _userManager.FindByNameAsync(currentUserId);
        var otherUser = await _userManager.FindByNameAsync(otherUserId);
        if (currentUser == null || otherUser == null)
        {
            return BadRequest(new { Message = "One or both users not found." });
        }

        // Check if a conversation already exists between the two users
        var existingConversation = await _context.UserConversations
            .Where(uc => uc.UserId == currentUser.Id && uc.Conversation.UserConversations.Any(ouc => ouc.UserId == otherUser.Id))
            .Include(uc => uc.Conversation)
            .FirstOrDefaultAsync();

        if (existingConversation != null)
        {
            return Ok(new ConversationDto
            {
                Id = existingConversation.ConversationId,
                Title = existingConversation.Title,
                CreatedBy = existingConversation.Conversation.CreatedBy,
                CreatedAt = existingConversation.Conversation.CreatedAt,
                UserNames = existingConversation.Conversation.UserConversations
                    .Select(uc => uc.User.UserName)
                    .ToList()
            });
        }

        // Create the new conversation with the current time
        var conversation = new Conversation
        {
            Title = $"Conversation between {currentUser.UserName} and {otherUser.UserName}",
            CreatedBy = currentUserId,
            CreatedAt = DateTime.UtcNow
        };

        // Create UserConversation objects with correct titles
        var currentUserConversation = new UserConversation
        {
            UserId = currentUser.Id,
            Title = $"{otherUser.UserName}",
            Conversation = conversation
        };

        var otherUserConversation = new UserConversation
        {
            UserId = otherUser.Id,
            Title = $"{currentUser.UserName}",
            Conversation = conversation
        };

        // Add the new conversation and user-conversations to the context
        _context.UserConversations.AddRange(currentUserConversation, otherUserConversation);
        await _context.SaveChangesAsync();

        // Return the details of the newly created conversation
        var conversationDto = new ConversationDto
        {
            Id = conversation.Id,
            Title = currentUserConversation.Title,
            CreatedBy = conversation.CreatedBy,
            CreatedAt = conversation.CreatedAt,
            UserNames = new List<string> { currentUser.UserName, otherUser.UserName }
        };

        return Ok(conversationDto);
    }

    [HttpGet("usersWithoutConversation")]
    public async Task<IActionResult> GetUsersWithoutConversation()
    {
        // Retrieve the current user's username from claims (string)
        var currentUserName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (currentUserName == null)
        {
            return Unauthorized(new { Message = "User not authenticated." });
        }

        // Fetch the actual user object to get the ID
        var currentUser = await _userManager.FindByNameAsync(currentUserName);
        if (currentUser == null)
        {
            return NotFound(new { Message = "Current user not found." });
        }

        var currentUserId = currentUser.Id;

        // Get all conversation IDs the current user is a part of
        var userConversationIds = await _context.UserConversations
            .Where(uc => uc.UserId == currentUserId)
            .Select(uc => uc.ConversationId)
            .ToListAsync();

        // Get the IDs of all users who are part of these conversations
        var userIdsWithConversation = await _context.UserConversations
            .Where(uc => userConversationIds.Contains(uc.ConversationId))
            .Select(uc => uc.UserId)
            .Distinct()
            .ToListAsync();

        var usersWithoutConversation = await _userManager.Users
            .Where(u => !userIdsWithConversation.Contains(u.Id) && u.Id != currentUserId)
            .ToListAsync();

        return Ok(usersWithoutConversation);
    }
    [HttpPost("sendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        var currentUserName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserName == null)
        {
            return Unauthorized(new { Message = "User not authenticated." });
        }
        var currentUser = await _userManager.FindByNameAsync(currentUserName);
        Console.WriteLine($"User: {currentUser}");
        if (currentUser == null)
        {
            return NotFound(new { Message = "Current user not found." });
        }

        var conversation = await _context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == request.ConversationId);
        Console.WriteLine($"Conversation: {conversation}");
        if (conversation == null)
        {
            return NotFound(new { Message = "Conversation not found." });
        }

        var newMessage = new ChatMessage
        {
            Message = request.Message,
            SenderId = currentUser.Id,
            Timestamp = DateTime.UtcNow,
            ConversationId = conversation.Id
        };
        Console.WriteLine($"Message: {newMessage}");

        _context.ChatMessages.Add(newMessage);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Message sent successfully." });
    }
    [HttpGet("{conversationId}/messages")]
    public async Task<IActionResult> GetMessages(int conversationId)
    {
        Console.WriteLine("Getting Messages");  // This should be printed when the method is hit
        
        var messages = await _context.ChatMessages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.Timestamp)
            .Select(m => new
            {
                m.Id,
                m.Message,
                m.Timestamp,
                Sender = m.Sender.UserName
            })
            .ToListAsync();

        Console.WriteLine($"Messages: {messages}");  // This should print the fetched messages

        return Ok(messages);
    }

}
