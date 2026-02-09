using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Entities.Payroll;

/// <summary>
/// PayrollRun entity for monthly payroll processing
/// </summary>
public class PayrollRun : AuditableEntity
{
    [Required]
    public int Month { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? RunCode { get; set; }

    public PayrollRunStatus Status { get; set; } = PayrollRunStatus.Draft;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalGrossSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalDeductions { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalNetSalary { get; set; }

    public int TotalEmployees { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public long? ProcessedById { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public long? ApprovedById { get; set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Navigation properties
    [ForeignKey(nameof(ApprovedById))]
    public virtual Employee? ApprovedBy { get; set; }

    public virtual ICollection<PayrollDetail> PayrollDetails { get; set; } = new List<PayrollDetail>();
}
