using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OnionArchitecture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    mail = table.Column<string>(type: "text", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    record_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by_id = table.Column<int>(type: "integer", nullable: true),
                    last_update_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "additionDocuments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    other = table.Column<string>(type: "text", nullable: true),
                    document_type = table.Column<int>(type: "integer", nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    record_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by_id = table.Column<int>(type: "integer", nullable: true),
                    last_update_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_additionDocuments", x => x.id);
                    table.ForeignKey(
                        name: "FK_additionDocuments_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "documents",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    additionDocument_id = table.Column<int>(type: "integer", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    record_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by_id = table.Column<int>(type: "integer", nullable: true),
                    last_update_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_documents_additionDocuments_additionDocument_id",
                        column: x => x.additionDocument_id,
                        principalTable: "additionDocuments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_mail",
                table: "users",
                column: "mail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_additionDocuments_customer_id",
                table: "additionDocuments",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_customers_mail",
                table: "customers",
                column: "mail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_documents_additionDocument_id",
                table: "documents",
                column: "additionDocument_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documents");

            migrationBuilder.DropTable(
                name: "additionDocuments");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropIndex(
                name: "IX_users_mail",
                table: "users");
        }
    }
}
