using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OptimConnect.Chat.Users;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.EntityFrameworkCore;
using OptimConnect.Chat.DataService;
using OptimConnect.Chat.Chatroom;
using OptimConnect.Chat.DTOs;

namespace OptimConnect.Chat.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }
        //         [HttpGet("conversations/{otherUserId}")]
        // public async Task<IActionResult> GetOrCreateConversation(string otherUserId)
        // {
        //     Console.WriteLine($"Received request for conversation with: {otherUserId}");

        //     // Retrieve the current user's username and object
        //     var currentUserName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //     var currentUser = await _userManager.FindByNameAsync(currentUserName);
        //     var otherUser = await _userManager.FindByNameAsync(otherUserId);

        //     Console.WriteLine($"Current User: {currentUser?.UserName ?? "null"}. Other User: {otherUser?.UserName ?? "null"}");

        //     if (currentUser == null || otherUser == null)
        //     {
        //         return BadRequest(new { Message = "One or both users not found." });
        //     }

        //     // Check if a conversation already exists between the two users
        //     var existingConversation = await _context.Conversations
        //         .Where(c => c.Users.Count == 2 &&
        //                     c.Users.Any(u => u.Id == currentUser.Id) &&
        //                     c.Users.Any(u => u.Id == otherUser.Id))
        //         .FirstOrDefaultAsync();

        //     if (existingConversation != null)
        //     {
        //         Console.WriteLine($"Existing conversation found: {existingConversation.Id}");
        //         // Conversation found, return it
        //         return Ok(existingConversation);
        //     }

        //     // If no conversation exists, create a new one
        //     var newConversation = new ChatRoom.Conversation
        //     {
        //         Title = "New Conversation",
        //         CreatedBy = currentUserName,
        //         Users = new List<User> { currentUser, otherUser }
        //     };

        //     Console.WriteLine($"New Conversation: {newConversation.Title} created by {newConversation.CreatedBy}");

        //     // Add the new conversation to the context and save
        //     _context.Conversations.Add(newConversation);
        //     await _context.SaveChangesAsync();

        //     var conversationDto = new ConversationDto
        //     {
        //         Id = newConversation.Id,
        //         Title = newConversation.Title,
        //         CreatedBy = newConversation.CreatedBy,
        //         CreatedAt = newConversation.CreatedAt,
        //         UserNames = newConversation.Users.Select(u => u.UserName).ToList()
        //     };

        //     // Fetch and log all conversations for the current user
        //     var conversations = await _context.Conversations
        //         .Where(c => c.Users.Any(u => u.Id == currentUser.Id))
        //         .Select(c => new ConversationDto
        //         {
        //             Id = c.Id,
        //             Title = c.Title,
        //             CreatedBy = c.CreatedBy,
        //             CreatedAt = c.CreatedAt,
        //             UserNames = c.Users.Select(u => u.UserName).ToList()
        //         })
        //         .ToListAsync();

        //     Console.WriteLine("Conversations for current user:");
        //     foreach (var thisConversation in conversations)
        //     {
        //         Console.WriteLine($"Conversation ID: {thisConversation.Id}, Title: {thisConversation.Title}, Users: {string.Join(", ", thisConversation.UserNames)}");
        //     }

        //     return Ok(conversationDto);
        // }

        [HttpGet("me")]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"User ID from JWT: {userId}");

            if (userId == null)
            {
                Console.WriteLine("User ID is null.");
                return Unauthorized(new { Message = "User not authenticated." });
            }

            Console.WriteLine($"Checking user ID: {userId}");
            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
            {
                Console.WriteLine("User not found in the database.");
                return NotFound(new { Message = "User not found." });
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage)
                                            .ToList();
                Console.WriteLine("ModelState errors: " + string.Join(", ", errors));
                return BadRequest(new { Errors = errors });
            }

            var user = new User { UserName = model.UserName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { Result = "Registration successful" });
            }

            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            // Log errors for debugging
            Console.WriteLine("UserManager errors: " + string.Join(", ", errorMessages));
            return BadRequest(new { Errors = errorMessages });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid request." });

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash!, model.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    // Password is correct
                    var token = GenerateJwtToken(user); // Generate JWT Token
                    Console.WriteLine($"Recognised {model.UserName}, {model.Password}");
                    return Ok(new { Message = "Login successful.", Token = token });
                }
                else
                {
                    Console.WriteLine("Password verification failed.");
                }
            }
            else
            {
                Console.WriteLine($"User not found: {model.UserName}");
            }

            // If we reach here, it means either username doesn't exist or password is incorrect
            Console.WriteLine("Unrecognised");
            return Unauthorized(new { Message = "Invalid username or password." });
        }

        public string GenerateJwtToken(User user)
        {
            // Validate user and handle null values
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            // Ensure that user.UserName and user.Id are not null
            var userName = user.UserName ?? "defaultUserName"; // Provide a default value if null
            var userId = user.Id ?? Guid.NewGuid().ToString(); // Provide a default value if null

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "defaultKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "defaultIssuer",
                audience: _configuration["Jwt:Audience"] ?? "defaultAudience",
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
