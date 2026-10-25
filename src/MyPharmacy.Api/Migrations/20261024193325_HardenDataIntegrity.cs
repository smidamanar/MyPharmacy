using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPharmacy.Api.Migrations
{
    /// <inheritdoc />
    public partial class HardenDataIntegrity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionItems_Medications_MedicationID",
                table: "PrescriptionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Patients_PatientID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Pharmacists_PharmacistID",
                table: "Prescriptions");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Prescriptions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Medications",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddCheckConstraint(
                name: "CK_PrescriptionItem_Quantity",
                table: "PrescriptionItems",
                sql: "[Quantity] > 0");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacists_Email",
                table: "Pharmacists",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_NationalId",
                table: "Patients",
                column: "NationalId",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Medication_Price",
                table: "Medications",
                sql: "[Price] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Medication_StockQuantity",
                table: "Medications",
                sql: "[StockQuantity] >= 0");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionItems_Medications_MedicationID",
                table: "PrescriptionItems",
                column: "MedicationID",
                principalTable: "Medications",
                principalColumn: "MedicationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Patients_PatientID",
                table: "Prescriptions",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Pharmacists_PharmacistID",
                table: "Prescriptions",
                column: "PharmacistID",
                principalTable: "Pharmacists",
                principalColumn: "PharmacistID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionItems_Medications_MedicationID",
                table: "PrescriptionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Patients_PatientID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Pharmacists_PharmacistID",
                table: "Prescriptions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PrescriptionItem_Quantity",
                table: "PrescriptionItems");

            migrationBuilder.DropIndex(
                name: "IX_Pharmacists_Email",
                table: "Pharmacists");

            migrationBuilder.DropIndex(
                name: "IX_Patients_NationalId",
                table: "Patients");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Medication_Price",
                table: "Medications");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Medication_StockQuantity",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Medications");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionItems_Medications_MedicationID",
                table: "PrescriptionItems",
                column: "MedicationID",
                principalTable: "Medications",
                principalColumn: "MedicationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Patients_PatientID",
                table: "Prescriptions",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Pharmacists_PharmacistID",
                table: "Prescriptions",
                column: "PharmacistID",
                principalTable: "Pharmacists",
                principalColumn: "PharmacistID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
