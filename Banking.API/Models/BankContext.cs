using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Banking.Models
{
    public class BankContext : IdentityDbContext<ApplicationUser>
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options)
        {
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardRequest> CardRequests { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanRequest> LoanRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankAccount>()
                .Property(b => b.AccountBalance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
        .HasOne(t => t.BankAccount)
        .WithMany(b => b.Transactions)
        .HasForeignKey(t => t.AccountId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
            .HasOne(t => t.RecipientAccount)
            .WithMany()
            .HasForeignKey(t => t.RecipientAccountId)
            .OnDelete(DeleteBehavior.Restrict);

            // Configure Card relationships to avoid cascade delete cycles
            modelBuilder.Entity<Card>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.BankAccount)
                .WithMany()
                .HasForeignKey(c => c.BankAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // Also restrict BankAccount -> User to avoid any potential issues
            modelBuilder.Entity<BankAccount>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Loan relationships
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}