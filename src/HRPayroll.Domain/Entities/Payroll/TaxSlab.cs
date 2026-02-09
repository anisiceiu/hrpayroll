using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities.Payroll;

/// <summary>
/// TaxSlab entity for defining tax slabs
/// </summary>
public class TaxSlab : AuditableEntity
{
    [Required]
    public long TaxConfigId { get; set; }

    [Required]
    public int SlabOrder { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MinIncome { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MaxIncome { get; set; }

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal TaxPercentage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal FixedAmount { get; set; } = 0;

    [MaxLength(500)]
    public string? Description { get; set; }

    // Navigation properties
    [ForeignKey(nameof(TaxConfigId))]
    public virtual TaxConfig? TaxConfig { get; set; }
}
