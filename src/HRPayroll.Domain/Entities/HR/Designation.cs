using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// Designation entity representing job titles/positions
/// </summary>
public class Designation : AuditableEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? NameBN { get; set; }

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    public long DepartmentId { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string? Grade { get; set; }

    [MaxLength(20)]
    public string? Level { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? BaseSalary { get; set; }

    public new bool IsActive { get; set; } = true;

    // Navigation properties
    [ForeignKey(nameof(DepartmentId))]
    public virtual Department? Department { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
