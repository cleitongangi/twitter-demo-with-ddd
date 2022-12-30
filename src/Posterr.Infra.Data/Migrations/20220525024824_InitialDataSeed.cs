using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posterr.Infra.Data.Migrations
{
    public partial class InitialDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "PostTypes",
                columns: new[] { "Id", "TypeDescription" },
                values: new object[,]
                {
                    { 1, "Post" },
                    { 2, "Repost" },
                    { 3, "Quote" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Users",
                columns: new[] { "Id", "FollowersCount", "FollowingCount", "JoinedAt", "MetricsUpdatedAt", "PostsCount", "Username" },
                values: new object[,]
                {
                    { 1L, 0, 0, new DateTime(2022, 5, 24, 23, 48, 24, 243, DateTimeKind.Local).AddTicks(2644), new DateTime(2022, 5, 24, 23, 48, 24, 243, DateTimeKind.Local).AddTicks(2654), 0, "cleiton.gangi" },
                    { 2L, 0, 0, new DateTime(2022, 5, 24, 23, 48, 24, 243, DateTimeKind.Local).AddTicks(2655), new DateTime(2022, 5, 24, 23, 48, 24, 243, DateTimeKind.Local).AddTicks(2655), 0, "user2" },
                    { 3L, 0, 0, new DateTime(2022, 5, 24, 23, 48, 24, 243, DateTimeKind.Local).AddTicks(2656), new DateTime(2022, 5, 24, 23, 48, 24, 243, DateTimeKind.Local).AddTicks(2657), 0, "user3" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
