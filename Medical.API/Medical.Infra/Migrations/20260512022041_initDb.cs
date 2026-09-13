#nullable disable

namespace Medical.Infra.Migrations;
/// <inheritdoc />
public partial class initDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AspNetRoles",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUsers",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Pwd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Patients",
            columns: table => new
            {
                PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PatientName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                Gender = table.Column<string>(type: "nvarchar(1)", nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                EmergencyContactName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                EmergencyContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Patients", x => x.PatientId);
            });

        migrationBuilder.CreateTable(
            name: "Suppliers",
            columns: table => new
            {
                SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SupplierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: ""),
                ContactPerson = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserLogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserRoles",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserTokens",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Bills",
            columns: table => new
            {
                BillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Discount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Tax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                FinalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Bills", x => x.BillId);
                table.ForeignKey(
                    name: "FK_Bills_Patients_PatientId",
                    column: x => x.PatientId,
                    principalTable: "Patients",
                    principalColumn: "PatientId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Medicines",
            columns: table => new
            {
                MedicineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MedicineName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                MedicineCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                MedicinePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Stock = table.Column<int>(type: "int", nullable: false),
                ExpirationDate = table.Column<DateOnly>(type: "date", nullable: false),
                SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ManufacturingDate = table.Column<DateOnly>(type: "date", nullable: false),
                Manufacturer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                SupplierId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Medicines", x => x.MedicineId);
                table.ForeignKey(
                    name: "FK_Medicines_Suppliers_SupplierId",
                    column: x => x.SupplierId,
                    principalTable: "Suppliers",
                    principalColumn: "SupplierId",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Medicines_Suppliers_SupplierId1",
                    column: x => x.SupplierId1,
                    principalTable: "Suppliers",
                    principalColumn: "SupplierId");
            });

        migrationBuilder.CreateTable(
            name: "BillItems",
            columns: table => new
            {
                BillItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MedicineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BillItems", x => x.BillItemId);
                table.ForeignKey(
                    name: "FK_BillItems_Bills_BillId",
                    column: x => x.BillId,
                    principalTable: "Bills",
                    principalColumn: "BillId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_BillItems_Medicines_MedicineId",
                    column: x => x.MedicineId,
                    principalTable: "Medicines",
                    principalColumn: "MedicineId",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "MedicineBatches",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MedicineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BatchNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                QuantityAvailable = table.Column<int>(type: "int", nullable: false),
                PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                SellingPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                ManufacturingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SupplierId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MedicineBatches", x => x.Id);
                table.ForeignKey(
                    name: "FK_MedicineBatches_Medicines_MedicineId",
                    column: x => x.MedicineId,
                    principalTable: "Medicines",
                    principalColumn: "MedicineId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MedicineBatches_Suppliers_SupplierId",
                    column: x => x.SupplierId,
                    principalTable: "Suppliers",
                    principalColumn: "SupplierId",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_MedicineBatches_Suppliers_SupplierId1",
                    column: x => x.SupplierId1,
                    principalTable: "Suppliers",
                    principalColumn: "SupplierId");
            });

        migrationBuilder.CreateTable(
            name: "InventoryTransactions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MedicineBatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MedicineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                table.ForeignKey(
                    name: "FK_InventoryTransactions_MedicineBatches_MedicineBatchId",
                    column: x => x.MedicineBatchId,
                    principalTable: "MedicineBatches",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "StockAdjustment",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MedicineBatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PreviousQuantity = table.Column<int>(type: "int", nullable: false),
                AdjustedQuantity = table.Column<int>(type: "int", nullable: false),
                Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                AdjustedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StockAdjustment", x => x.Id);
                table.ForeignKey(
                    name: "FK_StockAdjustment_MedicineBatches_MedicineBatchId",
                    column: x => x.MedicineBatchId,
                    principalTable: "MedicineBatches",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AspNetRoleClaims_RoleId",
            table: "AspNetRoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "AspNetRoles",
            column: "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserClaims_UserId",
            table: "AspNetUserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserLogins_UserId",
            table: "AspNetUserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserRoles_RoleId",
            table: "AspNetUserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "AspNetUsers",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "AspNetUsers",
            column: "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_BillItems_BillId",
            table: "BillItems",
            column: "BillId");

        migrationBuilder.CreateIndex(
            name: "IX_BillItems_MedicineId",
            table: "BillItems",
            column: "MedicineId");

        migrationBuilder.CreateIndex(
            name: "IX_Bills_PatientId",
            table: "Bills",
            column: "PatientId");

        migrationBuilder.CreateIndex(
            name: "IX_InventoryTransactions_MedicineBatchId",
            table: "InventoryTransactions",
            column: "MedicineBatchId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicineBatches_MedicineId",
            table: "MedicineBatches",
            column: "MedicineId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicineBatches_SupplierId",
            table: "MedicineBatches",
            column: "SupplierId");

        migrationBuilder.CreateIndex(
            name: "IX_MedicineBatches_SupplierId1",
            table: "MedicineBatches",
            column: "SupplierId1");

        migrationBuilder.CreateIndex(
            name: "IX_Medicines_SupplierId",
            table: "Medicines",
            column: "SupplierId");

        migrationBuilder.CreateIndex(
            name: "IX_Medicines_SupplierId1",
            table: "Medicines",
            column: "SupplierId1");

        migrationBuilder.CreateIndex(
            name: "IX_StockAdjustment_MedicineBatchId",
            table: "StockAdjustment",
            column: "MedicineBatchId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AspNetRoleClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserLogins");

        migrationBuilder.DropTable(
            name: "AspNetUserRoles");

        migrationBuilder.DropTable(
            name: "AspNetUserTokens");

        migrationBuilder.DropTable(
            name: "BillItems");

        migrationBuilder.DropTable(
            name: "InventoryTransactions");

        migrationBuilder.DropTable(
            name: "StockAdjustment");

        migrationBuilder.DropTable(
            name: "AspNetRoles");

        migrationBuilder.DropTable(
            name: "AspNetUsers");

        migrationBuilder.DropTable(
            name: "Bills");

        migrationBuilder.DropTable(
            name: "MedicineBatches");

        migrationBuilder.DropTable(
            name: "Patients");

        migrationBuilder.DropTable(
            name: "Medicines");

        migrationBuilder.DropTable(
            name: "Suppliers");
    }
}
