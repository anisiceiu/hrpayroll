using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// Attendance entity for tracking employee attendance
/// </summary>
public class Attendance : AuditableEntity
{
    [Required]
    public long EmployeeId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public TimeSpan? ClockInTime { get; set; }

    public TimeSpan? ClockOutTime { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    public decimal? WorkingHours { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    public decimal? OvertimeHours { get; set; }

    [Required]
    public AttendanceStatus Status { get; set; }

    public int? LateMinutes { get; set; }

    public int? EarlyLeavingMinutes { get; set; }

    [MaxLength(50)]
    public string? BiometricDeviceId { get; set; }

    [MaxLength(100)]
    public string? BiometricLogId { get; set; }

    [Required]
    public EntryType EntryType { get; set; } = EntryType.Manual;

    public long? ApprovedBy { get; set; }

    public DateTime? ApprovalDate { get; set; }

    [MaxLength(500)]
    public new string? Remarks { get; set; }

    // Navigation properties
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }

    [ForeignKey(nameof(ApprovedBy))]
    public virtual Employee? Approver { get; set; }
}
