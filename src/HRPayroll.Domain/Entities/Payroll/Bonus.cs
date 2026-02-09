using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Entities.Payroll;

/// <summary>
/// Bonus entity for managing employee bonuses
/// </summary>
public class Bonus : AuditableEntity
{
    [Required]
    public long EmployeeId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public BonusType Type { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime BonusDate { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public long? PayrollDetailId { get; set; }

    public bool IsTaxable { get; set; } = true;

    // Navigation properties
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey(nameof(PayrollDetailId))]
    public virtual PayrollDetail? PayrollDetail { get; set; }
}
