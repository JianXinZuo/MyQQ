using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WalletComponent.Migrations
{
    public partial class Init02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupRelationship_Groups_FromUserGroupId",
                table: "UserGroupRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupRelationship_Groups_ToUserGroupId",
                table: "UserGroupRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupRelationship_Users_UsersId1",
                table: "UserGroupRelationship");

            migrationBuilder.DropIndex(
                name: "IX_UserGroupRelationship_ToUserGroupId",
                table: "UserGroupRelationship");

            migrationBuilder.DropIndex(
                name: "IX_UserGroupRelationship_UsersId1",
                table: "UserGroupRelationship");

            migrationBuilder.DropColumn(
                name: "UsersId1",
                table: "UserGroupRelationship");

            migrationBuilder.RenameColumn(
                name: "ToUserGroupId",
                table: "UserGroupRelationship",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "FromUserGroupId",
                table: "UserGroupRelationship",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroupRelationship_FromUserGroupId",
                table: "UserGroupRelationship",
                newName: "IX_UserGroupRelationship_GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupRelationship_Groups_GroupId",
                table: "UserGroupRelationship",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroupRelationship_Groups_GroupId",
                table: "UserGroupRelationship");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserGroupRelationship",
                newName: "ToUserGroupId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "UserGroupRelationship",
                newName: "FromUserGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroupRelationship_GroupId",
                table: "UserGroupRelationship",
                newName: "IX_UserGroupRelationship_FromUserGroupId");

            migrationBuilder.AddColumn<Guid>(
                name: "UsersId1",
                table: "UserGroupRelationship",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRelationship_ToUserGroupId",
                table: "UserGroupRelationship",
                column: "ToUserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRelationship_UsersId1",
                table: "UserGroupRelationship",
                column: "UsersId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupRelationship_Groups_FromUserGroupId",
                table: "UserGroupRelationship",
                column: "FromUserGroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupRelationship_Groups_ToUserGroupId",
                table: "UserGroupRelationship",
                column: "ToUserGroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroupRelationship_Users_UsersId1",
                table: "UserGroupRelationship",
                column: "UsersId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
