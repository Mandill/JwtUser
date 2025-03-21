using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtUser.Migrations
{
    /// <inheritdoc />
    public partial class AddProductIdonProductImageTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_images",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    image_data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    image_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    product_id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_imag__3213E83FB3E1F904", x => x.id);
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_images");
        }
    }
}
