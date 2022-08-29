using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReStore.Svc.Data.Migrations
{
  public partial class PaymentIntentAdded : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
          name: "PaymentIntentId",
          table: "Orders",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "ClientSecret",
          table: "Baskets",
          type: "TEXT",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "PaymentIntentId",
          table: "Baskets",
          type: "TEXT",
          nullable: true);

      migrationBuilder.UpdateData(
          table: "AspNetRoles",
          keyColumn: "Id",
          keyValue: 1,
          column: "ConcurrencyStamp",
          value: "59063380-e77a-4831-9488-a0516b0ab477");

      migrationBuilder.UpdateData(
          table: "AspNetRoles",
          keyColumn: "Id",
          keyValue: 2,
          column: "ConcurrencyStamp",
          value: "f3527c88-0939-406a-8c35-f04cd3a6d16b");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "PaymentIntentId",
          table: "Orders");

      migrationBuilder.DropColumn(
          name: "ClientSecret",
          table: "Baskets");

      migrationBuilder.DropColumn(
          name: "PaymentIntentId",
          table: "Baskets");

      migrationBuilder.UpdateData(
          table: "AspNetRoles",
          keyColumn: "Id",
          keyValue: 1,
          column: "ConcurrencyStamp",
          value: "fbb3ea8d-d412-4827-bd73-14d4f2ee63d9");

      migrationBuilder.UpdateData(
          table: "AspNetRoles",
          keyColumn: "Id",
          keyValue: 2,
          column: "ConcurrencyStamp",
          value: "304bb08c-bcc3-4be3-896d-8f620a727063");
    }
  }
}