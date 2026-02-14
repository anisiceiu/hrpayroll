using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities.Payroll;

/// <summary>
/// Represents tax rebate for investments
/// </summary>
public class TaxRebate : AuditableEntity
{
    public new long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal MinInvestment { get; set; }
    public decimal MaxInvestment { get; set; }
    public decimal RebatePercentage { get; set; }
    public int FinancialYear { get; set; }
    public new bool IsActive { get; set; }
}
