using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantesAspNet.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStartDateAndFoodType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoodType",
                table: "Restaurants",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Restaurants",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodType",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Restaurants");
        }
    }
}
