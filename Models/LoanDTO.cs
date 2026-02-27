using System.ComponentModel.DataAnnotations;
using Banking.Models;

public class LoanDTO
    {
        public int Id { get; set; }

        [Required]
        public decimal PrincipalAmount { get; set; }

        [Required]
        public decimal InterestRate { get; set; } = 12.0m; // Default interest rate

        [Required]
        [Range(1, 120)]
        public int DurationInMonths { get; set; }

        [Required]
        public decimal MonthlyInstallment { get; set; }

        public LoanStatus Status { get; set; } = LoanStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedAt { get; set; }
        public DateTime? DisbursedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

    }
