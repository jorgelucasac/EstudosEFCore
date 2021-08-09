using Microsoft.EntityFrameworkCore.Migrations;

namespace Estudos.EFCore.MultiTenant.Migrations
{
    public partial class EstrategiaMultSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Pessoas",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Pessoas",
                columns: new[] { "Id", "Nome", "TenantId" },
                values: new object[,]
                {
                    { 1, "Pessoa 1", "tenant-1" },
                    { 2, "Pessoa 2", "tenant-2" },
                    { 3, "Pessoa 3", "tenant-2" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Produtos",
                columns: new[] { "Id", "Descricao", "TenantId" },
                values: new object[,]
                {
                    { 1, "Descricao 1", "tenant-1" },
                    { 2, "Descricao 2", "tenant-2" },
                    { 3, "Descricao 3", "tenant-2" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pessoas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Produtos",
                schema: "dbo");
        }
    }
}
