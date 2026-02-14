using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Entities.Payroll;

/// <summary>
/// Loan entity for managing employee loans
/// </summary>
public class Loan : AuditableEntity
{
    [Required]
    public long EmployeeId { get; set; }

    [Required]
    [MaxLength(50)]
    public string LoanNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public LoanType LoanType { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrincipalAmount { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal InterestRate { get; set; }

    public int NumberOfInstallments { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal InstallmentAmount { get; set; }

    public DateTime LoanDate { get; set; }

    public DateTime? StartRepaymentDate { get; set; }

    public LoanStatus Status { get; set; } = LoanStatus.Pending;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public new string? Remarks { get; set; }

    public long? ApprovedById { get; set; }

    public DateTime? ApprovalDate { get; set; }

    // Navigation properties
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey(nameof(ApprovedById))]
    public virtual Employee? ApprovedBy { get; set; }

    public virtual ICollection<LoanRepayment> Repayments { get; set; } = new List<LoanRepayment>();
}

/// <summary>
/// LoanRepayment entity for tracking loan repayments
/// </summary>
public class LoanRepayment : AuditableEntity
{
    [Required]
    public long LoanId { get; set; }

    [Required]
    public int InstallmentNumber { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrincipalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal InterestAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public DateTime RepaymentDate { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    [MaxLength(500)]
    public new string? Remarks { get; set; }

    // Navigation properties
    [ForeignKey(nameof(LoanId))]
    public virtual Loan Loan { get; set; } = null!;
}
