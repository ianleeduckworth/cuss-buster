using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace CussBuster.Core.Data.Entities
{
    public partial class CussBusterContext : DbContext
    {
		private readonly IConfiguration _configuration;

        public virtual DbSet<CallLog> CallLog { get; set; }
        public virtual DbSet<SearchType> SearchType { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Word> Word { get; set; }
        public virtual DbSet<WordAudit> WordAudit { get; set; }
        public virtual DbSet<WordType> WordType { get; set; }

		public CussBusterContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CussBusterDatabase"));
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CallLog>(entity =>
            {
                entity.ToTable("CallLog", "usr");

                entity.Property(e => e.EventDate).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CallLog)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallLog_User");
            });

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

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "usr");

                entity.Property(e => e.CanCallApi).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
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

                entity.HasIndex(e => e.BadWord)
                    .HasName("AK_Word_Word")
                    .IsUnique();

                entity.Property(e => e.BadWord)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

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
