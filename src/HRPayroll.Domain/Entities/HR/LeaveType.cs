using System.ComponentModel.DataAnnotations;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// LeaveType entity for defining different types of leave
/// </summary>
public class LeaveType : AuditableEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? NameBN { get; set; }

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsPaidLeave { get; set; } = true;

    public bool IsCarryForwardAllowed { get; set; } = false;

    public int? MaxCarryForwardDays { get; set; }

    public int? MaxAccumulationDays { get; set; }

    public bool RequiresApproval { get; set; } = true;

    public bool RequiresDocument { get; set; } = false;

    [MaxLength(10)]
    public string? ColorCode { get; set; }

    public new bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();
    public virtual ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();
}
