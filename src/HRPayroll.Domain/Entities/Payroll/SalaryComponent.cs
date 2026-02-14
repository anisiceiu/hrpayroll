using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Entities.Payroll;

/// <summary>
/// SalaryComponent entity for defining salary components
/// </summary>
public class SalaryComponent : AuditableEntity
{
    [Required]
    public long SalaryStructureId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public ComponentType ComponentType { get; set; }

    [Required]
    public CalculationType CalculationType { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Amount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? PercentageOf { get; set; }

    [MaxLength(500)]
    public string? Formula { get; set; }

    public bool IsTaxable { get; set; } = true;

    public bool IsPfApplicable { get; set; } = false;

    public bool IsGfApplicable { get; set; } = false;

    public int DisplayOrder { get; set; } = 0;

    public new bool IsActive { get; set; } = true;

    // Navigation properties
    [ForeignKey(nameof(SalaryStructureId))]
    public virtual SalaryStructure? SalaryStructure { get; set; }
}
