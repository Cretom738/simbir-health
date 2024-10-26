using System;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:role", "admin,manager,doctor,user");

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    last_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    first_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    username = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    roles = table.Column<Role[]>(type: "role[]", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token_pair_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    expired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    account_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_sessions_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "id", "first_name", "last_name", "password", "roles", "username" },
                values: new object[,]
                {
                    { 1L, "admin", "admin", "$argon2id$v=19$m=32768,t=4,p=1$254Tzhn1PgSUajYIGisD/w$b/wDyeGQpS6Zrj4YXAOJFCiqKS2mGrvMBI22GkqZhDA", new[] { Role.Admin }, "admin" },
                    { 2L, "manager", "manager", "$argon2id$v=19$m=32768,t=4,p=1$VNSKhe5/fbPopKk8Pr0QTg$1G8aMpo98NLyJN9hEdh3pu7PyPDS0Kx7LM7lIPVMd5Y", new[] { Role.Manager }, "manager" },
                    { 3L, "doctor", "doctor", "$argon2id$v=19$m=32768,t=4,p=1$vgIeXjNyoXwGBZJt/lQ8Sg$gqPUMn1Z6uXn4CTer8vvBq5XFCsSVWgOsp1/CrzfALo", new[] { Role.Doctor }, "doctor" },
                    { 4L, "user", "user", "$argon2id$v=19$m=32768,t=4,p=1$vq8oDBMSvtw2CEJKdq91zQ$/0gM1yEhZi/yBbMIOGkPuqt7CWf+21YjPJACrO6IW3s", new[] { Role.User }, "user" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_username",
                table: "accounts",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sessions_account_id",
                table: "sessions",
                column: "account_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "accounts");
        }
    }
}
