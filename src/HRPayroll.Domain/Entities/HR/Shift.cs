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

    public int BreakTimeMinutes { get; set; } = 60;

    [Column(TypeName = "decimal(4,2)")]
    public decimal WorkingHours { get; set; }

    public int GraceTimeMinutes { get; set; } = 0;

    public bool IsNightShift { get; set; } = false;

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
