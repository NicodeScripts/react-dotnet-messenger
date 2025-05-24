using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptimConnect.chat.Migrations
{
    /// <inheritdoc />
    public partial class AddUserConversations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_AspNetUsers_UsersId",
                table: "UserConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_Conversations_ConversationsId",
                table: "UserConversations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserConversations",
                table: "UserConversations");

            migrationBuilder.DropIndex(
                name: "IX_UserConversations_UsersId",
                table: "UserConversations");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Conversations");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "UserConversations",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ConversationsId",
                table: "UserConversations",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "UserConversations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "UserConversations",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Conversations",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserConversations",
                table: "UserConversations",
                columns: new[] { "UserId", "ConversationId" });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConversationId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserConversations_ConversationId",
                table: "UserConversations",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ConversationId",
                table: "Message",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_AspNetUsers_UserId",
                table: "UserConversations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_Conversations_ConversationId",
                table: "UserConversations",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_AspNetUsers_UserId",
                table: "UserConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_Conversations_ConversationId",
                table: "UserConversations");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserConversations",
                table: "UserConversations");

            migrationBuilder.DropIndex(
                name: "IX_UserConversations_ConversationId",
                table: "UserConversations");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "UserConversations");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "UserConversations");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserConversations",
                newName: "ConversationsId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserConversations",
                newName: "UsersId");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Conversations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Conversations",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserConversations",
                table: "UserConversations",
                columns: new[] { "ConversationsId", "UsersId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserConversations_UsersId",
                table: "UserConversations",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_AspNetUsers_UsersId",
                table: "UserConversations",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_Conversations_ConversationsId",
                table: "UserConversations",
                column: "ConversationsId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
