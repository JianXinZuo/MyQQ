using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WalletComponent.Migrations
{
    public partial class Init01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    UserCode = table.Column<string>(nullable: true),
                    PassWord = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    HeadImg = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Sex = table.Column<bool>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    IdCard = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FriendNotification",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FromUserId = table.Column<Guid>(nullable: false),
                    ToUserId = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendNotification_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FriendNotification_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GroupName = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupRelationship",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FromUserGroupId = table.Column<Guid>(nullable: false),
                    ToUserGroupId = table.Column<Guid>(nullable: false),
                    UsersId = table.Column<Guid>(nullable: true),
                    UsersId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupRelationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroupRelationship_Groups_FromUserGroupId",
                        column: x => x.FromUserGroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroupRelationship_Groups_ToUserGroupId",
                        column: x => x.ToUserGroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupRelationship_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroupRelationship_Users_UsersId1",
                        column: x => x.UsersId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendNotification_FromUserId",
                table: "FriendNotification",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendNotification_ToUserId",
                table: "FriendNotification",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRelationship_FromUserGroupId",
                table: "UserGroupRelationship",
                column: "FromUserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRelationship_ToUserGroupId",
                table: "UserGroupRelationship",
                column: "ToUserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRelationship_UsersId",
                table: "UserGroupRelationship",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRelationship_UsersId1",
                table: "UserGroupRelationship",
                column: "UsersId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendNotification");

            migrationBuilder.DropTable(
                name: "UserGroupRelationship");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
