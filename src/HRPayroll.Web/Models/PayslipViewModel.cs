using HRPayroll.Domain.Dtos;

namespace HRPayroll.Web.Models;

/// <summary>
/// View model for displaying payslip information
/// </summary>
public class PayslipViewModel : PayslipDto
{
    // Helper method to format currency
    public string FormatCurrency(decimal amount)
    {
        return amount.ToString("N2");
    }
}
