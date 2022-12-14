using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTLASPMVC.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiangViens",
                columns: table => new
                {
                    giangvienid = table.Column<string>(name: "giangvien_id", type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    phone = table.Column<string>(type: "TEXT", nullable: false),
                    gender = table.Column<string>(type: "TEXT", nullable: false),
                    birthday = table.Column<DateTime>(type: "TEXT", nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "TEXT", nullable: false),
                    updatedat = table.Column<DateTime>(name: "updated_at", type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiangViens", x => x.giangvienid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false),
                    fullname = table.Column<string>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "TEXT", nullable: false),
                    updatedat = table.Column<DateTime>(name: "updated_at", type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Lops",
                columns: table => new
                {
                    lopid = table.Column<string>(name: "lop_id", type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    giangvienid = table.Column<string>(name: "giangvien_id", type: "TEXT", nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "TEXT", nullable: false),
                    updatedat = table.Column<DateTime>(name: "updated_at", type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lops", x => x.lopid);
                    table.ForeignKey(
                        name: "FK_Lops_GiangViens_giangvien_id",
                        column: x => x.giangvienid,
                        principalTable: "GiangViens",
                        principalColumn: "giangvien_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sinhviens",
                columns: table => new
                {
                    sinhvienid = table.Column<string>(name: "sinhvien_id", type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    phone = table.Column<string>(type: "TEXT", nullable: false),
                    gender = table.Column<string>(type: "TEXT", nullable: false),
                    birthday = table.Column<DateTime>(type: "TEXT", nullable: false),
                    lopid = table.Column<string>(name: "lop_id", type: "TEXT", nullable: false),
                    createdat = table.Column<DateTime>(name: "created_at", type: "TEXT", nullable: false),
                    updatedat = table.Column<DateTime>(name: "updated_at", type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sinhviens", x => x.sinhvienid);
                    table.ForeignKey(
                        name: "FK_Sinhviens_Lops_lop_id",
                        column: x => x.lopid,
                        principalTable: "Lops",
                        principalColumn: "lop_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lops_giangvien_id",
                table: "Lops",
                column: "giangvien_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sinhviens_lop_id",
                table: "Sinhviens",
                column: "lop_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sinhviens");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Lops");

            migrationBuilder.DropTable(
                name: "GiangViens");
        }
    }
}
