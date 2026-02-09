using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// Leave entity for employee leave requests
/// </summary>
public class Leave : AuditableEntity
{
    [Required]
    public long EmployeeId { get; set; }

    [Required]
    public long LeaveTypeId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(4,2)")]
    public decimal TotalDays { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }

    [Required]
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    [Required]
    public DateTime AppliedOn { get; set; } = DateTime.Now;

    public long? ApprovedBy { get; set; }

    public DateTime? ApprovalDate { get; set; }

    [MaxLength(500)]
    public string? ApprovalRemarks { get; set; }

    public bool IsHalfDay { get; set; } = false;

    public HalfDayPortion? HalfDayPortion { get; set; }

    [MaxLength(255)]
    public string? SupportingDocument { get; set; }

    [MaxLength(500)]
    public string? CancellationReason { get; set; }

    public long? CancelledBy { get; set; }

    public DateTime? CancelledDate { get; set; }

    // Navigation properties
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }

    [ForeignKey(nameof(LeaveTypeId))]
    public virtual LeaveType? LeaveType { get; set; }

    [ForeignKey(nameof(ApprovedBy))]
    public virtual Employee? Approver { get; set; }

    [ForeignKey(nameof(CancelledBy))]
    public virtual Employee? Canceller { get; set; }
}
