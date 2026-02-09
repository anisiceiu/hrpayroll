using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Entities.Payroll;

/// <summary>
/// Overtime entity for managing employee overtime
/// </summary>
public class Overtime : AuditableEntity
{
    [Required]
    public long EmployeeId { get; set; }

    [Required]
    public DateTime OvertimeDate { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal TotalHours { get; set; }

    public OvertimeStatus Status { get; set; } = OvertimeStatus.Pending;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? Remarks { get; set; }

    public long? ApprovedById { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public long? PayrollDetailId { get; set; }

    // Navigation properties
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey(nameof(ApprovedById))]
    public virtual Employee? ApprovedBy { get; set; }

    [ForeignKey(nameof(PayrollDetailId))]
    public virtual PayrollDetail? PayrollDetail { get; set; }
}
