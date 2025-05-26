using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83771022-503f-4deb-850b-e77d94f3fad9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1aac5c8-9a30-4027-a268-ad5bbb6d56d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5ce1387-b80f-449e-84d9-2b9ec93975b4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f810fb3-c4a9-4e31-8310-f4653ea305cb", null, "Staff", "STAFF" },
                    { "691a23d1-c8ad-47b1-86c9-738ff6f5a952", null, "Customer", "CUSTOMER" },
                    { "7cb03284-7bee-4f17-b8cc-92d809999f97", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f810fb3-c4a9-4e31-8310-f4653ea305cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "691a23d1-c8ad-47b1-86c9-738ff6f5a952");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7cb03284-7bee-4f17-b8cc-92d809999f97");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "83771022-503f-4deb-850b-e77d94f3fad9", null, "Admin", "ADMIN" },
                    { "e1aac5c8-9a30-4027-a268-ad5bbb6d56d5", null, "Customer", "CUSTOMER" },
                    { "f5ce1387-b80f-449e-84d9-2b9ec93975b4", null, "Staff", "STAFF" }
                });
        }
    }
}
