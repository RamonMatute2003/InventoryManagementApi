using InventoryManagement.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Persistence.Contexts;

public partial class InventoryDbContext : DbContext
{
    public InventoryDbContext()
    {
    }

    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<InventoryLot> InventoryLots { get; set; }

    public virtual DbSet<InventoryOutDetail> InventoryOutDetails { get; set; }

    public virtual DbSet<InventoryOutHeader> InventoryOutHeaders { get; set; }

    public virtual DbSet<InventoryOutStatus> InventoryOutStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:inventorymanagementserverfarsiman.database.windows.net,1433;Database=InventoryManagement;User ID=ariel;Password=Matute10;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.IdBranch).HasName("PK__Branches__54205B04EE612F58");

            entity.Property(e => e.BranchLocation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BranchName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InventoryLot>(entity =>
        {
            entity.HasKey(e => e.IdBatch).HasName("PK__Inventor__2CBAA72950ED2749");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.InventoryLots)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK_InventoryLots_Products");
        });

        modelBuilder.Entity<InventoryOutDetail>(entity =>
        {
            entity.HasKey(e => e.IdOutDetail).HasName("PK__Inventor__EF40CC7AC06796F0");

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdBatchNavigation).WithMany(p => p.InventoryOutDetails)
                .HasForeignKey(d => d.IdBatch)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryOutDetails_InventoryLots");

            entity.HasOne(d => d.IdOutHeaderNavigation).WithMany(p => p.InventoryOutDetails)
                .HasForeignKey(d => d.IdOutHeader)
                .HasConstraintName("FK_InventoryOutDetails_InventoryOutHeaders");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.InventoryOutDetails)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK_InventoryOutDetails_Products");
        });

        modelBuilder.Entity<InventoryOutHeader>(entity =>
        {
            entity.HasKey(e => e.IdOutHeader).HasName("PK__Inventor__E0A673DEDD56DB2A");

            entity.Property(e => e.IdStatus).HasDefaultValue(1);
            entity.Property(e => e.OutDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdBranchNavigation).WithMany(p => p.InventoryOutHeaders)
                .HasForeignKey(d => d.IdBranch)
                .HasConstraintName("FK_InventoryOutHeaders_Branches");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.InventoryOutHeaders)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("FK_InventoryOutHeaders_InventoryOutStatus");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.InventoryOutHeaderIdUserNavigations)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryOutHeaders_Users");

            entity.HasOne(d => d.ReceivedByNavigation).WithMany(p => p.InventoryOutHeaderReceivedByNavigations)
                .HasForeignKey(d => d.ReceivedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_InventoryOutHeaders_ReceivedBy");
        });

        modelBuilder.Entity<InventoryOutStatus>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK__Inventor__B450643A7CAC9482");

            entity.ToTable("InventoryOutStatus");

            entity.HasIndex(e => e.StatusName, "UQ__Inventor__05E7698A319BBB40").IsUnique();

            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Products__2E8946D49AC3FB54");

            entity.HasIndex(e => e.ProductCode, "UQ__Products__2F4E024F4523F705").IsUnique();

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProductCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdBranchNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdBranch)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Products_Branches");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Roles__B43690542DB71A6A");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B616027470B34").IsUnique();

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__B7C926388B95EA19");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F284567198EED5").IsUnique();

            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
