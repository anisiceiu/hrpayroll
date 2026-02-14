using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities.Payroll;

/// <summary>
/// TaxConfig entity for tax configuration
/// </summary>
public class TaxConfig : AuditableEntity
{
    [Required]
    public int TaxYear { get; set; }

    [MaxLength(50)]
    public string? Country { get; set; } = "Bangladesh";

    [MaxLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasicTaxFreeLimit { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RebateLimit { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MinimumTaxableIncome { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal InvestmentRebatePercentage { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal FemaleRebatePercentage { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal SeniorCitizenRebatePercentage { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal DisabledRebatePercentage { get; set; }

    [Required]
    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public new bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<TaxSlab> TaxSlabs { get; set; } = new List<TaxSlab>();
}
