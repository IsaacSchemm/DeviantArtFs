using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeviantArtFs.Stash.Marshal.Examples.StashInterface.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeltaCursors",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    Cursor = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeltaCursors", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "StashEntries",
                columns: table => new
                {
                    StashEntryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<long>(nullable: true),
                    StackId = table.Column<long>(nullable: false),
                    MetadataJson = table.Column<string>(nullable: false),
                    Position = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StashEntries", x => x.StashEntryId);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccessToken = table.Column<string>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeltaCursors");

            migrationBuilder.DropTable(
                name: "StashEntries");

            migrationBuilder.DropTable(
                name: "Tokens");
        }
    }
}
