using FinancialService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialService.Data
{
    public class FinancialContext:DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public FinancialContext(DbContextOptions<FinancialContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId);
                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.AccountType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Currency).IsRequired().HasMaxLength(3); // ISO currency code
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TransactionDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsCredit);
                entity.Property(e => e.TransactionType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);

                entity.HasOne<Account>()
                    .WithMany()
                    .HasForeignKey(t => t.AccountId)
                    .OnDelete(DeleteBehavior.Cascade); // Ensures referential integrity
            });
        }


    }
}
