// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;
// using OptimConnect.Chat.Chatroom;
// using OptimConnect.Chat.Users;

// namespace OptimConnect.Chat.DataService
// {
//     public class ApplicationDbContext : IdentityDbContext<User>
//     {
//         public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

//         public DbSet<ChatRoom> ChatRooms { get; set; } = null!;
//         public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
//         public DbSet<Conversation> Conversations { get; set; }
//         public DbSet<UserConversation> UserConversations { get; set; } // New DbSet for UserConversation

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);

//             // Configure ChatRoom
//             modelBuilder.Entity<ChatRoom>(entity =>
//             {
//                 entity.HasKey(cr => cr.Id);
//                 entity.Property(cr => cr.Name).IsRequired().HasMaxLength(100);
//             });

//             // Configure ChatMessage
//             modelBuilder.Entity<ChatMessage>(entity =>
//             {
//                 entity.HasKey(cm => cm.Id);
//                 entity.Property(cm => cm.Message).IsRequired();
//                 entity.HasOne(cm => cm.ChatRoom)
//                     .WithMany(cr => cr.ChatMessages)
//                     .HasForeignKey(cm => cm.ChatRoomId);
//             });

//             modelBuilder.Entity<Conversation>()
//                 .HasMany(c => c.Users)
//                 .WithMany(u => u.Conversations)
//                 .UsingEntity<UserConversation>(
//                     j => j
//                         .HasOne(uc => uc.User)
//                         .WithMany(u => u.UserConversations)
//                         .HasForeignKey(uc => uc.UserId),
//                     j => j
//                         .HasOne(uc => uc.Conversation)
//                         .WithMany(c => c.UserConversations)
//                         .HasForeignKey(uc => uc.ConversationId),
//                     j => 
//                     {
//                         j.HasKey(uc => new { uc.UserId, uc.ConversationId }); // Composite key
//                         j.Property(uc => uc.Title).HasMaxLength(100); // Optional
//                     }
//                 );

//             modelBuilder.Entity<UserConversation>(entity =>
//             {
//                 entity.HasKey(uc => new { uc.UserId, uc.ConversationId });

//                 entity.HasOne(uc => uc.User)
//                     .WithMany(u => u.UserConversations)
//                     .HasForeignKey(uc => uc.UserId);

//                 entity.HasOne(uc => uc.Conversation)
//                     .WithMany(c => c.UserConversations)
//                     .HasForeignKey(uc => uc.ConversationId);

//                 entity.Property(uc => uc.Title).IsRequired().HasMaxLength(100);
//             });

//             modelBuilder.Entity<User>(entity =>
//             {
//                 entity.HasKey(u => u.Id);
//                 entity.HasIndex(u => u.NormalizedUserName).IsUnique();
//             });
//         }
//     }
// }


using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OptimConnect.Chat.Chatroom;
using OptimConnect.Chat.Users;

namespace OptimConnect.Chat.DataService
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ChatRoom> ChatRooms { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<Conversation> Conversations { get; set; } = null!;
        public DbSet<UserConversation> UserConversations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ChatRoom
            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.HasKey(cr => cr.Id);
                entity.Property(cr => cr.Name).IsRequired().HasMaxLength(100);
            });

            // Configure ChatMessage
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(cm => cm.Id);
                entity.Property(cm => cm.Id).ValueGeneratedOnAdd();
                entity.Property(cm => cm.Message).IsRequired();
                entity.Property(cm => cm.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(cm => cm.Sender)
                    .WithMany(u => u.Messages)
                    .HasForeignKey(cm => cm.SenderId);

                entity.HasOne(cm => cm.Conversation)
                    .WithMany(c => c.Messages)
                    .HasForeignKey(cm => cm.ConversationId);
            });

            // Define many-to-many relationship with extra fields using UserConversation
            modelBuilder.Entity<Conversation>()
                .HasMany(c => c.UserConversations)
                .WithOne(uc => uc.Conversation)
                .HasForeignKey(uc => uc.ConversationId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserConversations)
                .WithOne(uc => uc.User)
                .HasForeignKey(uc => uc.UserId);

            // Define the UserConversation entity
            modelBuilder.Entity<UserConversation>(entity =>
            {
                entity.HasKey(uc => new { uc.UserId, uc.ConversationId }); // Composite key

                entity.HasOne(uc => uc.User)
                    .WithMany(u => u.UserConversations)
                    .HasForeignKey(uc => uc.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(uc => uc.Conversation)
                    .WithMany(c => c.UserConversations)
                    .HasForeignKey(uc => uc.ConversationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(uc => uc.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
