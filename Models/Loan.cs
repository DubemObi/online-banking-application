using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class Loan
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
        public ApplicationUser User { get; set; }
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        public void CalculateMonthlyInstallment()
        {
            var monthlyRate = InterestRate / 100m / 12m;

            if (monthlyRate == 0)
            {
                MonthlyInstallment = PrincipalAmount / DurationInMonths;
                return;
            }

            var power = (decimal)Math.Pow((double)(1 + monthlyRate), DurationInMonths);

            MonthlyInstallment =
                PrincipalAmount *
                monthlyRate *
                power /
                (power - 1);
        }
    }


    public enum LoanStatus
    {
        Pending,
        Approved,
        Rejected,
        Active,
        Completed,
        Defaulted
    }
}