using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DissertationProject.Migrations
{
    public partial class transactions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyTransaction_Families_FamilyId",
                table: "FamilyTransaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FamilyTransaction",
                table: "FamilyTransaction");

            migrationBuilder.RenameTable(
                name: "FamilyTransaction",
                newName: "FamilyTransactions");

            migrationBuilder.RenameIndex(
                name: "IX_FamilyTransaction_FamilyId",
                table: "FamilyTransactions",
                newName: "IX_FamilyTransactions_FamilyId");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "FamilyTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamilyTransactions",
                table: "FamilyTransactions",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1a4df6c2-e479-40eb-8135-d492174424f2",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAEAACcQAAAAEM+V2m1lqaxAy4CxfMT5Dsedr5I+9AbaoKhIQ+pzdg3kO8re2rfXF6y1TRz39JEieQ==", "51888769-2ddb-4090-948e-f68b671fbd0c" });

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyTransactions_Families_FamilyId",
                table: "FamilyTransactions",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyTransactions_Families_FamilyId",
                table: "FamilyTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FamilyTransactions",
                table: "FamilyTransactions");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "FamilyTransactions");

            migrationBuilder.RenameTable(
                name: "FamilyTransactions",
                newName: "FamilyTransaction");

            migrationBuilder.RenameIndex(
                name: "IX_FamilyTransactions_FamilyId",
                table: "FamilyTransaction",
                newName: "IX_FamilyTransaction_FamilyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamilyTransaction",
                table: "FamilyTransaction",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1a4df6c2-e479-40eb-8135-d492174424f2",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAEAACcQAAAAEKGO0zO5vyOcz5C4IHyjoAm7SLqns8AKVHHajRg9ItWTrlng4aggPEkNul/CQgkKyg==", "9a2e80bb-92e7-4436-bc61-d234d5aa9901" });

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyTransaction_Families_FamilyId",
                table: "FamilyTransaction",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
