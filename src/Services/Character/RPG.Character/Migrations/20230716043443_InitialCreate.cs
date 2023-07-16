using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG.Character.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RaceTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1250)", maxLength: 1250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BaseAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PhisycalDamageLow = table.Column<int>(type: "int", nullable: false),
                    PhisycalDamageHigh = table.Column<int>(type: "int", nullable: false),
                    CriticRatePercentage = table.Column<int>(type: "int", nullable: false),
                    BaseHealth = table.Column<int>(type: "int", nullable: false),
                    MagicalDamageLow = table.Column<int>(type: "int", nullable: false),
                    MagicalDamageHigh = table.Column<int>(type: "int", nullable: false),
                    Concentration = table.Column<int>(type: "int", nullable: false),
                    DodgePercentage = table.Column<int>(type: "int", nullable: false),
                    Armor = table.Column<int>(type: "int", nullable: false),
                    MagicalArmor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseAttributes_ClassTypes_ClassTypeId",
                        column: x => x.ClassTypeId,
                        principalTable: "ClassTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseAttributes_RaceTypes_RaceTypeId",
                        column: x => x.RaceTypeId,
                        principalTable: "RaceTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<long>(type: "bigint", nullable: false),
                    ProfessionLevel = table.Column<int>(type: "int", nullable: false),
                    ProfessionExperience = table.Column<int>(type: "int", nullable: false),
                    ClassTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RaceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Characters_ClassTypes_ClassTypeId",
                        column: x => x.ClassTypeId,
                        principalTable: "ClassTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_RaceTypes_RaceTypeId",
                        column: x => x.RaceTypeId,
                        principalTable: "RaceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseAttributes_ClassTypeId",
                table: "BaseAttributes",
                column: "ClassTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseAttributes_RaceTypeId",
                table: "BaseAttributes",
                column: "RaceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ClassTypeId",
                table: "Characters",
                column: "ClassTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_RaceTypeId",
                table: "Characters",
                column: "RaceTypeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseAttributes");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "ClassTypes");

            migrationBuilder.DropTable(
                name: "RaceTypes");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
