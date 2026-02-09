namespace HRPayroll.Domain.Dtos;

/// <summary>
/// Data transfer object for payslip information
/// </summary>
public class PayslipDto
{
    // Employee Information
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeCode { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public DateTime? JoinDate { get; set; }
    
    // Bank Details
    public string BankName { get; set; } = string.Empty;
    public string BankAccountNo { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    
    // Pay Period
    public int Month { get; set; }
    public int Year { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int WorkingDays { get; set; }
    public int DaysPresent { get; set; }
    public int DaysAbsent { get; set; }
    
    // Earnings
    public decimal BasicSalary { get; set; }
    public decimal HouseRentAllowance { get; set; }
    public decimal TransportAllowance { get; set; }
    public decimal MedicalAllowance { get; set; }
    public decimal OvertimeAmount { get; set; }
    public decimal TotalEarnings { get; set; }
    
    // Deductions
    public decimal ProvidentFund { get; set; }
    public decimal TaxDeduction { get; set; }
    public decimal Insurance { get; set; }
    public decimal OtherDeductions { get; set; }
    public decimal TotalDeductions { get; set; }
    
    // Summary
    public decimal GrossSalary { get; set; }
    public decimal NetSalary { get; set; }
}
