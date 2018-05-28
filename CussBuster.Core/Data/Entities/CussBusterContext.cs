using Microsoft.EntityFrameworkCore;

namespace CussBuster.Core.Data.Entities
{
	public partial class CussBusterContext : DbContext
    {
        public virtual DbSet<SearchType> SearchType { get; set; }
        public virtual DbSet<Word> Word { get; set; }
        public virtual DbSet<WordAudit> WordAudit { get; set; }
        public virtual DbSet<WordType> WordType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS01;Database=CussBuster;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchType>(entity =>
            {
                entity.ToTable("SearchType", "static");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Word>(entity =>
            {
                entity.ToTable("Word", "core");

                entity.HasIndex(e => e.Word1)
                    .HasName("AK_Word_Word")
                    .IsUnique();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.SearchTypeId).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Word1)
                    .IsRequired()
                    .HasColumnName("Word")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.SearchType)
                    .WithMany(p => p.Word)
                    .HasForeignKey(d => d.SearchTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Word_SearchType");

                entity.HasOne(d => d.WordType)
                    .WithMany(p => p.Word)
                    .HasForeignKey(d => d.WordTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Word_WordType");
            });

            modelBuilder.Entity<WordAudit>(entity =>
            {
                entity.ToTable("WordAudit", "aud");

                entity.Property(e => e.EventDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<WordType>(entity =>
            {
                entity.ToTable("WordType", "static");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
