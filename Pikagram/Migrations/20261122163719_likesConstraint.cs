using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pikagram.Migrations
{
    /// <inheritdoc />
    public partial class likesConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Comments_LikedId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_LikedId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_LikedId",
                table: "Likes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Likes_LikedId",
                table: "Likes",
                column: "LikedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Comments_LikedId",
                table: "Likes",
                column: "LikedId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_LikedId",
                table: "Likes",
                column: "LikedId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
