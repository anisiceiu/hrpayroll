using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Entities.Payroll;

/// <summary>
/// PayrollDetail entity for individual employee payroll in a payroll run
/// </summary>
public class PayrollDetail : AuditableEntity
{
    [Required]
    public long PayrollRunId { get; set; }

    [Required]
    public long EmployeeId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasicSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalEarnings { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalDeductions { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? OvertimeAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? BonusAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TaxDeduction { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TaxAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ProvidentFund { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? LoanDeduction { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? OtherDeductions { get; set; }

    [MaxLength(500)]
    public new string? Remarks { get; set; }

    public int? WorkingDays { get; set; }

    public int? PaidDays { get; set; }

    [MaxLength(50)]
    public string? BankAccountNo { get; set; }

    [MaxLength(100)]
    public string? BankName { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }

    // Navigation properties
    [ForeignKey(nameof(PayrollRunId))]
    public virtual PayrollRun PayrollRun { get; set; } = null!;

    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; } = null!;
}
