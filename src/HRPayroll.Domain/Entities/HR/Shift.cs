using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// Shift entity for managing work schedules
/// </summary>
public class Shift : AuditableEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Break duration in minutes
    /// </summary>
    public int BreakTimeMinutes { get; set; } = 60;

    /// <summary>
    /// Minimum working hours required for this shift
    /// </summary>
    [Column(TypeName = "decimal(4,2)")]
    public decimal? MinimumWorkingHours { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    public decimal WorkingHours { get; set; }

    /// <summary>
    /// Grace period in minutes for late arrival
    /// </summary>
    public int GraceTimeMinutes { get; set; } = 0;

    /// <summary>
    /// Indicates if this is a night shift
    /// </summary>
    public bool IsNightShift { get; set; } = false;

    /// <summary>
    /// Indicates if this is a flexible shift (variable start/end times)
    /// </summary>
    public bool IsFlexible { get; set; } = false;

    /// <summary>
    /// Indicates if shift spans overnight (end time < start time)
    /// </summary>
    public bool IsOvernight { get; set; } = false;

    public new bool IsActive { get; set; } = true;

    /// <summary>
    /// Additional notes or description
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    // Navigation properties
    public virtual ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
