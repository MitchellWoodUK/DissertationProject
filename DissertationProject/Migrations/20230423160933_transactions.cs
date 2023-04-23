using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DissertationProject.Migrations
{
    public partial class transactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FamilyTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FamilyId = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionType = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyTransaction_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1a4df6c2-e479-40eb-8135-d492174424f2",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAEAACcQAAAAEKGO0zO5vyOcz5C4IHyjoAm7SLqns8AKVHHajRg9ItWTrlng4aggPEkNul/CQgkKyg==", "9a2e80bb-92e7-4436-bc61-d234d5aa9901" });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyTransaction_FamilyId",
                table: "FamilyTransaction",
                column: "FamilyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyTransaction");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1a4df6c2-e479-40eb-8135-d492174424f2",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAEAACcQAAAAELkXlx0/iMnqfvQcqyZMMKOrHqcqBnMUIsYpcWU3PRbV9PX2H0lVoKVa/uHq5f30Sw==", "ce58b958-cfd2-4cf7-9276-214a442c2bfa" });
        }
    }
}
