using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// LeaveBalance entity for tracking employee leave balances
/// </summary>
public class LeaveBalance : AuditableEntity
{
    [Required]
    public long EmployeeId { get; set; }

    [Required]
    public long LeaveTypeId { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    [Column(TypeName = "decimal(4,2)")]
    public decimal TotalDays { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    public decimal UsedDays { get; set; } = 0;

    [Column(TypeName = "decimal(4,2)")]
    public decimal PendingDays { get; set; } = 0;

    [Column(TypeName = "decimal(4,2)")]
    public decimal CarryForwardDays { get; set; } = 0;

    [NotMapped]
    public decimal AvailableDays => TotalDays - UsedDays - PendingDays + CarryForwardDays;

    // Navigation properties
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }

    [ForeignKey(nameof(LeaveTypeId))]
    public virtual LeaveType? LeaveType { get; set; }
}
