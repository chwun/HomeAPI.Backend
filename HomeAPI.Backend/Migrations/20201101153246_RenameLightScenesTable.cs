using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeAPI.Backend.Migrations
{
	[ExcludeFromCodeCoverage]
	public partial class RenameLightScenesTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameTable(
				name: "Lighting_Scenes",
				newName: "LightScenes"
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameTable(
				name: "LightScenes",
				newName: "Lighting_Scenes"
			);
		}
	}
}
