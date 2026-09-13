namespace Medical.Infra.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<SupplierEntity> Suppliers { get; set; }

    public DbSet<PatientEntity> Patients { get; set; }

    public DbSet<MedicineEntity> Medicines { get; set; }

    public DbSet<BillEntity> Bills { get; set; }

    public DbSet<BillItemEntity> BillItems { get; set; }

    public DbSet<MedicineBatchEntity> MedicineBatches { get; set; }

    public DbSet<InventoryTransactionEntity> InventoryTransactions { get; set; }

    public DbSet<StockAdjustmentEntity> StockAdjustment { get; set; }

    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

    public DbSet<AuditLogEntity> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Call base to register Identity entities (User, Role, etc.)
        base.OnModelCreating(modelBuilder);

        // Configure SupplierEntity
        modelBuilder.Entity<SupplierEntity>(entity =>
        {
            entity.HasKey(s => s.SupplierId);
            entity.Property(s => s.SupplierName)
                .HasMaxLength(255)
                .IsRequired()
                .HasDefaultValue("");
            entity.Property(s => s.ContactPerson).HasMaxLength(255);
            entity.Property(s => s.Email).HasMaxLength(255);
            entity.Property(s => s.PhoneNumber).HasMaxLength(20);
            entity.Property(s => s.Address).HasMaxLength(500);
            entity.Property(s => s.City).HasMaxLength(100);
            entity.Property(s => s.State).HasMaxLength(100);
            entity.Property(s => s.PostalCode).HasMaxLength(20);
            entity.Property(s => s.Country).HasMaxLength(100);
            entity.Property(s => s.CreatedAt).IsRequired();
            entity.Property(s => s.LastUpdatedAt).IsRequired();
            entity.Property(s => s.IsActive).HasDefaultValue(true);

            // Relationship: One supplier has many medicines
            entity.HasMany(s => s.Medicines)
                .WithOne(m => m.Supplier)
                .HasForeignKey(m => m.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PatientEntity
        modelBuilder.Entity<PatientEntity>(entity =>
        {
            entity.HasKey(p => p.PatientId);
            entity.Property(p => p.PatientName).HasMaxLength(255).IsRequired();
            entity.Property(p => p.PhoneNumber).HasMaxLength(20);
            entity.Property(p => p.Email).HasMaxLength(255);
            entity.Property(p => p.Address).HasMaxLength(500);
            entity.Property(p => p.City).HasMaxLength(100);
            entity.Property(p => p.State).HasMaxLength(100);
            entity.Property(p => p.PostalCode).HasMaxLength(20);
            entity.Property(p => p.Country).HasMaxLength(100);
            entity.Property(p => p.EmergencyContactName).HasMaxLength(255);
            entity.Property(p => p.EmergencyContactPhone).HasMaxLength(20);

            // Relationship: One patient has many bills
            entity.HasMany(p => p.Bills)
                .WithOne(b => b.Patient)
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure MedicineEntity
        modelBuilder.Entity<MedicineEntity>(entity =>
        {
            entity.HasKey(m => m.MedicineId);
            entity.Property(m => m.MedicineName).HasMaxLength(255).IsRequired();
            entity.Property(m => m.MedicineCategory).HasMaxLength(100);
            entity.Property(m => m.Manufacturer).HasMaxLength(255);
            entity.Property(m => m.MedicinePrice).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(m => m.Stock).IsRequired();
            entity.Property(m => m.ExpirationDate).IsRequired();
            entity.Property(m => m.ManufacturingDate).IsRequired();
            entity.Property(m => m.CreatedAt).IsRequired();
            entity.Property(m => m.LastUpdatedAt).IsRequired();

            // Foreign key relationship to SupplierEntity
            entity.HasOne(m => m.Supplier)
                .WithMany(s => s.Medicines)
                .HasForeignKey(m => m.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure BillEntity
        modelBuilder.Entity<BillEntity>(entity =>
        {
            // Use BillId (Guid) as primary key instead of int Id
            entity.HasKey(b => b.BillId);

            entity.Property(b => b.PatientId).IsRequired();
            entity.Property(b => b.TotalAmount).HasPrecision(18, 2).IsRequired();
            entity.Property(b => b.Discount).HasPrecision(18, 2).IsRequired();
            entity.Property(b => b.Tax).HasPrecision(18, 2).IsRequired();
            entity.Property(b => b.FinalAmount).HasPrecision(18, 2).IsRequired();

            // Relationship: One bill has many items
            entity.HasMany(b => b.BillItems)
                .WithOne(bi => bi.Bill)
                .HasForeignKey(bi => bi.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Bill has foreign key to Patient
            entity.HasOne(b => b.Patient)
                .WithMany(p => p.Bills)
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure BillItemEntity
        modelBuilder.Entity<BillItemEntity>(entity =>
        {
            // Use BillItemId as primary key
            entity.HasKey(bi => bi.BillItemId);

            entity.Property(bi => bi.BillId).IsRequired();
            entity.Property(bi => bi.MedicineId).IsRequired();
            entity.Property(bi => bi.UnitPrice).HasPrecision(18, 2).IsRequired();
            entity.Property(bi => bi.TotalPrice).HasPrecision(18, 2).IsRequired();

            // Relationship: BillItem has foreign key to Bill
            entity.HasOne(bi => bi.Bill)
                .WithMany(b => b.BillItems)
                .HasForeignKey(bi => bi.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: BillItem has foreign key to Medicine
            entity.HasOne(bi => bi.Medicine)
                .WithMany(m => m.BillItems)
                .HasForeignKey(bi => bi.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure MedicineBatchEntity
        modelBuilder.Entity<MedicineBatchEntity>(entity =>
        {
            entity.HasKey(b => b.Id);

            entity.Property(b => b.MedicineId).IsRequired();
            entity.Property(b => b.BatchNumber).HasMaxLength(100);
            entity.Property(b => b.PurchasePrice).HasPrecision(18, 2);
            entity.Property(b => b.SellingPrice).HasPrecision(18, 2);

            // Relationship: Many batches belong to one medicine
            entity.HasOne(b => b.Medicine)
                .WithMany(m => m.Batches)
                .HasForeignKey(b => b.MedicineId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: One batch has many inventory transactions
            entity.HasMany(b => b.Transactions)
                .WithOne(t => t.Batch)
                .HasForeignKey(t => t.MedicineBatchId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: One batch has many stock adjustments
            entity.HasMany(b => b.StockAdjustments)
                .WithOne(s => s.Batch)
                .HasForeignKey(s => s.MedicineBatchId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure InventoryTransactionEntity
        modelBuilder.Entity<InventoryTransactionEntity>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.MedicineBatchId).IsRequired();
            entity.Property(t => t.MedicineId).IsRequired();
            entity.Property(t => t.ReferenceNumber).HasMaxLength(100);

            // Relationship: Transaction has foreign key to MedicineBatch
            entity.HasOne(t => t.Batch)
                .WithMany(b => b.Transactions)
                .HasForeignKey(t => t.MedicineBatchId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure StockAdjustmentEntity
        modelBuilder.Entity<StockAdjustmentEntity>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.MedicineBatchId).IsRequired();
            entity.Property(a => a.Reason).HasMaxLength(255);

            // Relationship: Adjustment has foreign key to MedicineBatch
            entity.HasOne(a => a.Batch)
                .WithMany(b => b.StockAdjustments)
                .HasForeignKey(a => a.MedicineBatchId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Supplier - Medicine (one-to-many)
        modelBuilder.Entity<MedicineEntity>()
            .HasOne<SupplierEntity>()
            .WithMany(s => s.Medicines)
            .HasForeignKey(m => m.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // Supplier - MedicineBatch (one-to-many)
        modelBuilder.Entity<MedicineBatchEntity>()
            .HasOne<SupplierEntity>()
            .WithMany()
            .HasForeignKey(b => b.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure RefreshTokenEntity
        modelBuilder.Entity<RefreshTokenEntity>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Token).IsRequired().HasMaxLength(500);
            entity.Property(r => r.JwtId).IsRequired().HasMaxLength(500);
            entity.Property(r => r.CreatedAt).IsRequired();
            entity.Property(r => r.ExpiresAt).IsRequired();
            entity.Property(r => r.RevokedByIp).HasMaxLength(50);
            entity.Property(r => r.ReplacedByToken).HasMaxLength(500);
            entity.Property(r => r.UserId).HasMaxLength(450);

            // Relationship: One user has many refresh tokens
            entity.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index on Token for quick lookup
            entity.HasIndex(r => r.Token).IsUnique();
            entity.HasIndex(r => r.UserId);
        });

        // Configure AuditLogEntity
        modelBuilder.Entity<AuditLogEntity>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.UserId).IsRequired().HasMaxLength(450);
            entity.Property(a => a.UserName).IsRequired().HasMaxLength(255);
            entity.Property(a => a.Action).IsRequired().HasMaxLength(100);
            entity.Property(a => a.EntityName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.EntityId).HasMaxLength(450);
            entity.Property(a => a.OldValues);
            entity.Property(a => a.NewValues);
            entity.Property(a => a.IpAddress).HasMaxLength(50);
            entity.Property(a => a.UserAgent).HasMaxLength(500);
            entity.Property(a => a.CreatedAt).IsRequired();
            entity.Property(a => a.OrganizationId).HasMaxLength(450);

            // Indexes for common queries
            entity.HasIndex(a => a.UserId);
            entity.HasIndex(a => a.Action);
            entity.HasIndex(a => a.EntityName);
            entity.HasIndex(a => a.CreatedAt);
        });
    }

}
