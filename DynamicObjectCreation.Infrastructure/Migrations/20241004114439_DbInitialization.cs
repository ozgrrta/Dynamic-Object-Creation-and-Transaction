using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicObjectCreation.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class DbInitialization : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "DynamicObjects",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					ObjectType = table.Column<string>(type: "text", nullable: false),
					Data = table.Column<JsonDocument>(type: "jsonb", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					IsActive = table.Column<bool>(type: "boolean", nullable: false),
					IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_DynamicObjects", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "ValidationRules",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					ObjectType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
					PropertyName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
					RuleType = table.Column<int>(type: "integer", nullable: false),
					RuleValue = table.Column<string>(type: "text", nullable: false),
					ParentPropertyName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
					ChildObjectType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
					CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					IsActive = table.Column<bool>(type: "boolean", nullable: false),
					IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ValidationRules", x => x.Id);
				});

			migrationBuilder.CreateIndex(
				name: "idx_data_gin",
				table: "DynamicObjects",
				column: "Data")
				.Annotation("Npgsql:IndexMethod", "GIN");

			migrationBuilder.CreateIndex(
				name: "IX_DynamicObjects_CreatedAt",
				table: "DynamicObjects",
				column: "CreatedAt");

			migrationBuilder.CreateIndex(
				name: "IX_DynamicObjects_ObjectType",
				table: "DynamicObjects",
				column: "ObjectType");

			migrationBuilder.CreateIndex(
				name: "IX_ValidationRules_ObjectType",
				table: "ValidationRules",
				column: "ObjectType");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "DynamicObjects");

			migrationBuilder.DropTable(
				name: "ValidationRules");
		}
	}
}
