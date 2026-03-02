namespace Banking.Models
{
    public class LoanResponseDTO
    {
        public int Id { get; set; }

        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int DurationInMonths { get; set; }

        public decimal MonthlyInstallment { get; set; }

        public LoanStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? DisbursedAt { get; set; }

        public int UserId { get; set; }
        public int BankAccountId { get; set; }
    }
}