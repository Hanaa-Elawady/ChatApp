using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConnectionsEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnection_AspNetUsers_ApplicationUserId",
                table: "UserConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConnection_AspNetUsers_User1Id",
                table: "UserConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConnection_AspNetUsers_User2Id",
                table: "UserConnection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserConnection",
                table: "UserConnection");

            migrationBuilder.RenameTable(
                name: "UserConnection",
                newName: "UserConnections");

            migrationBuilder.RenameIndex(
                name: "IX_UserConnection_User2Id",
                table: "UserConnections",
                newName: "IX_UserConnections_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_UserConnection_User1Id",
                table: "UserConnections",
                newName: "IX_UserConnections_User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_UserConnection_ApplicationUserId",
                table: "UserConnections",
                newName: "IX_UserConnections_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserConnections",
                table: "UserConnections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_AspNetUsers_ApplicationUserId",
                table: "UserConnections",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_AspNetUsers_User1Id",
                table: "UserConnections",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_AspNetUsers_User2Id",
                table: "UserConnections",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_AspNetUsers_ApplicationUserId",
                table: "UserConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_AspNetUsers_User1Id",
                table: "UserConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_AspNetUsers_User2Id",
                table: "UserConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserConnections",
                table: "UserConnections");

            migrationBuilder.RenameTable(
                name: "UserConnections",
                newName: "UserConnection");

            migrationBuilder.RenameIndex(
                name: "IX_UserConnections_User2Id",
                table: "UserConnection",
                newName: "IX_UserConnection_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_UserConnections_User1Id",
                table: "UserConnection",
                newName: "IX_UserConnection_User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_UserConnections_ApplicationUserId",
                table: "UserConnection",
                newName: "IX_UserConnection_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserConnection",
                table: "UserConnection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnection_AspNetUsers_ApplicationUserId",
                table: "UserConnection",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnection_AspNetUsers_User1Id",
                table: "UserConnection",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnection_AspNetUsers_User2Id",
                table: "UserConnection",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
