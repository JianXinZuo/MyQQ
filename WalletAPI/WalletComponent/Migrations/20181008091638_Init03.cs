using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WalletComponent.Migrations
{
    public partial class Init03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupRelationship_Users_UsersId",
                table: "UserGroupRelationship");

            migrationBuilder.DropIndex(
                name: "IX_UserGroupRelationship_UsersId",
                table: "UserGroupRelationship");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "UserGroupRelationship");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRelationship_UserId",
                table: "UserGroupRelationship",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupRelationship_Users_UserId",
                table: "UserGroupRelationship",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupRelationship_Users_UserId",
                table: "UserGroupRelationship");

            migrationBuilder.DropIndex(
                name: "IX_UserGroupRelationship_UserId",
                table: "UserGroupRelationship");

            migrationBuilder.AddColumn<Guid>(
                name: "UsersId",
                table: "UserGroupRelationship",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRelationship_UsersId",
                table: "UserGroupRelationship",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupRelationship_Users_UsersId",
                table: "UserGroupRelationship",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
