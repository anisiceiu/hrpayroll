using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// Department entity representing organizational departments
/// </summary>
public class Department : AuditableEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? NameBN { get; set; }

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public long? ParentDepartmentId { get; set; }

    public long? HeadOfDepartmentId { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    [ForeignKey(nameof(ParentDepartmentId))]
    public virtual Department? ParentDepartment { get; set; }

    [ForeignKey(nameof(HeadOfDepartmentId))]
    public virtual Employee? HeadOfDepartment { get; set; }

    public virtual ICollection<Department> SubDepartments { get; set; } = new List<Department>();
    public virtual ICollection<Designation> Designations { get; set; } = new List<Designation>();
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
