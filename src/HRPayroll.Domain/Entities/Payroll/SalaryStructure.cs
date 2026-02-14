using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Entities.HR;

namespace HRPayroll.Domain.Entities.Payroll;

/// <summary>
/// SalaryStructure entity for managing employee salary structures
/// </summary>
public class SalaryStructure : AuditableEntity
{
    [Required]
    public long EmployeeId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasicSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal HouseRentAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TransportAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MedicalAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ConveyanceAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OtherAllowances { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal ProvidentFundPercentage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxDeduction { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OtherDeductions { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalDeductions { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetSalary { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public new bool IsActive { get; set; } = true;

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Navigation properties
   
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; } = null!;

    public virtual ICollection<SalaryComponent> Components { get; set; } = new List<SalaryComponent>();
}
