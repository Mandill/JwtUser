using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtUser.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenTableChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_refreshtoken",
                columns: table => new
                {
                    userid = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    tokenid = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    refreshtoken = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    expiretime = table.Column<DateTime>(type: "datetime", nullable: true),
                    TblUserUserId = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_refreshtoken", x => new { x.userid, x.tokenid });
                    table.ForeignKey(
                        name: "FK_tbl_refreshtoken_tbl_users_TblUserUserId",
                        column: x => x.TblUserUserId,
                        principalTable: "tbl_users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_refreshtoken_TblUserUserId",
                table: "tbl_refreshtoken",
                column: "TblUserUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_refreshtoken");
        }
    }
}