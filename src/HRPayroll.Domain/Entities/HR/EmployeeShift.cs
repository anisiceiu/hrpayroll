using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// EmployeeShift entity for managing employee shift assignments
/// </summary>
public class EmployeeShift : AuditableEntity
{
    /// <summary>
    /// Employee ID
    /// </summary>
    public long EmployeeId { get; set; }

    /// <summary>
    /// Shift ID
    /// </summary>
    public long ShiftId { get; set; }

    /// <summary>
    /// Date from which this shift assignment is effective
    /// </summary>
    public DateTime EffectiveFrom { get; set; }

    /// <summary>
    /// Date until which this shift assignment is effective (null = indefinite)
    /// </summary>
    public DateTime? EffectiveTo { get; set; }

    /// <summary>
    /// Indicates if this is the current active assignment
    /// </summary>
    public new bool IsActive { get; set; } = true;

    /// <summary>
    /// Additional notes or remarks
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    // Navigation properties
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }

    [ForeignKey(nameof(ShiftId))]
    public virtual Shift? Shift { get; set; }
}
