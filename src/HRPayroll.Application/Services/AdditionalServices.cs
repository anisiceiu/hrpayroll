using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Entities.Payroll;
using HRPayroll.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace HRPayroll.Application.Services;

/// <summary>
/// Tax service implementation for Bangladesh tax rules
/// </summary>
public class TaxService : ITaxService
{
    private readonly ITaxConfigRepository _taxConfigRepository;
    private readonly ITaxSlabRepository _taxSlabRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public TaxService(
        ITaxConfigRepository taxConfigRepository,
        ITaxSlabRepository taxSlabRepository,
        IEmployeeRepository employeeRepository)
    {
        _taxConfigRepository = taxConfigRepository;
        _taxSlabRepository = taxSlabRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<decimal> CalculateMonthlyTaxAsync(long employeeId, int month, int year)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
        {
            throw new Exception("Employee not found.");
        }

        // Get annual salary (simplified - should get from payroll)
        var annualSalary = 600000m; // Example: 50,000 monthly

        // Calculate annual tax
        var annualTax = await CalculateAnnualTaxAsync(employeeId, year);

        // Return monthly tax
        return Math.Round(annualTax / 12, 2);
    }

    public async Task<decimal> CalculateAnnualTaxAsync(long employeeId, int year)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
        {
            throw new Exception("Employee not found.");
        }

        if (employee.IsTaxExempted)
        {
            return 0;
        }

        var taxConfig = await _taxConfigRepository.GetByTaxYearAsync(year);
        if (taxConfig == null)
        {
            throw new Exception("Tax configuration not found for the specified year.");
        }

        var slabs = await _taxSlabRepository.GetByConfigIdAsync(taxConfig.Id);
        if (!slabs.Any())
        {
            throw new Exception("Tax slabs not configured.");
        }

        // Simplified annual income (should come from salary data)
        var annualIncome = 600000m;

        // Apply exemptions
        var exemptions = taxConfig.BasicTaxFreeLimit;
        var taxableIncome = Math.Max(0, annualIncome - exemptions);

        // Calculate tax using slabs
        var tax = CalculateTaxBySlabs(taxableIncome, slabs.ToList());

        // Apply rebate (simplified)
        if (employee.Gender == Domain.Enums.Gender.Female)
        {
            tax = tax * (1 - taxConfig.FemaleRebatePercentage / 100);
        }

        return Math.Round(tax, 2);
    }

    private decimal CalculateTaxBySlabs(decimal income, List<TaxSlab> slabs)
    {
        var tax = 0m;
        var remainingIncome = income;
        var sortedSlabs = slabs.OrderBy(s => s.MinIncome).ToList();

        foreach (var slab in sortedSlabs)
        {
            if (remainingIncome <= 0) break;

            var slabRange = slab.MaxIncome.HasValue
                ? slab.MaxIncome.Value - slab.MinIncome
                : decimal.MaxValue;

            var taxableInSlab = Math.Min(remainingIncome, slabRange);

            tax += taxableInSlab * (slab.TaxPercentage / 100);
            remainingIncome -= taxableInSlab;
        }

        return Math.Max(0, tax);
    }

    public async Task<TaxConfig?> GetCurrentTaxConfigAsync()
    {
        return await _taxConfigRepository.GetCurrentConfigAsync();
    }

    public async Task<TaxConfig> CreateTaxConfigAsync(TaxConfig taxConfig)
    {
        return await _taxConfigRepository.AddAsync(taxConfig);
    }

    public async Task<TaxConfig> UpdateTaxConfigAsync(TaxConfig taxConfig)
    {
        var existing = await _taxConfigRepository.GetByIdAsync(taxConfig.Id);
        if (existing == null)
        {
            throw new Exception("Tax configuration not found.");
        }

        existing.TaxYear = taxConfig.TaxYear;
        existing.BasicTaxFreeLimit = taxConfig.BasicTaxFreeLimit;
        existing.FemaleRebatePercentage = taxConfig.FemaleRebatePercentage;
        existing.SeniorCitizenRebatePercentage = taxConfig.SeniorCitizenRebatePercentage;
        existing.IsActive = taxConfig.IsActive;

        return await _taxConfigRepository.UpdateAsync(existing);
    }

    public async Task<IEnumerable<TaxSlab>> GetCurrentTaxSlabsAsync()
    {
        return await _taxSlabRepository.GetCurrentTaxSlabsAsync();
    }
}

/// <summary>
/// Report service implementation
/// </summary>
public class ReportService : IReportService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IPayrollDetailRepository _payrollDetailRepository;

    public ReportService(
        IEmployeeRepository employeeRepository,
        IAttendanceRepository attendanceRepository,
        IPayrollDetailRepository payrollDetailRepository)
    {
        _employeeRepository = employeeRepository;
        _attendanceRepository = attendanceRepository;
        _payrollDetailRepository = payrollDetailRepository;
    }

    public async Task<byte[]> GenerateEmployeeReportAsync(long? departmentId = null)
    {
        var employees = await _employeeRepository.GetAllAsync();
        if (departmentId.HasValue)
        {
            employees = employees.Where(e => e.DepartmentId == departmentId.Value);
        }

        // Generate CSV (simplified)
        var csv = new System.Text.StringBuilder();
        csv.AppendLine("EmployeeCode,Name,Email,Department,Designation,Status,DateOfJoining");
        foreach (var emp in employees)
        {
            csv.AppendLine($"{emp.EmployeeCode},{emp.FirstName} {emp.LastName},{emp.Email},{emp.Department?.Name},{emp.Designation?.Name},{emp.Status},{emp.DateOfJoining:d}");
        }

        return System.Text.Encoding.UTF8.GetBytes(csv.ToString());
    }

    public async Task<byte[]> GenerateAttendanceReportAsync(DateTime startDate, DateTime endDate, long? departmentId = null)
    {
        // Simplified - should filter by department
        var attendances = await _attendanceRepository.GetAllAsync();
        attendances = attendances.Where(a => a.Date >= startDate && a.Date <= endDate);

        var csv = new System.Text.StringBuilder();
        csv.AppendLine("Date,Employee,Status,ClockIn,ClockOut,WorkingHours");
        foreach (var att in attendances)
        {
            csv.AppendLine($"{att.Date:d},{att.Employee?.FirstName} {att.Employee?.LastName},{att.Status},{att.ClockInTime},{att.ClockOutTime},{att.WorkingHours}");
        }

        return System.Text.Encoding.UTF8.GetBytes(csv.ToString());
    }

    public async Task<byte[]> GeneratePayrollSummaryAsync(int month, int year)
    {
        var details = await _payrollDetailRepository.GetAllAsync();
        details = details.Where(d => d.CreatedAt.Year == year && d.CreatedAt.Month == month);

        var csv = new System.Text.StringBuilder();
        csv.AppendLine("Employee,GrossSalary,TotalDeductions,NetSalary,PaymentStatus");
        foreach (var detail in details)
        {
            csv.AppendLine($"{detail.Employee?.FirstName} {detail.Employee?.LastName},{detail.GrossSalary},{detail.TotalDeductions},{detail.NetSalary},{detail.PaymentStatus}");
        }

        return System.Text.Encoding.UTF8.GetBytes(csv.ToString());
    }

    public async Task<byte[]> GenerateTaxReportAsync(int year)
    {
        // Simplified - should calculate actual tax
        var employees = await _employeeRepository.GetActiveEmployeesAsync();

        var csv = new System.Text.StringBuilder();
        csv.AppendLine("EmployeeCode,Name,AnnualIncome,TaxAmount,TaxExempted");
        foreach (var emp in employees)
        {
            csv.AppendLine($"{emp.EmployeeCode},{emp.FirstName} {emp.LastName},600000,{(emp.IsTaxExempted ? "0" : "25000")},{emp.IsTaxExempted}");
        }

        return System.Text.Encoding.UTF8.GetBytes(csv.ToString());
    }

    public async Task<byte[]> GenerateBankTransferFileAsync(long payrollRunId)
    {
        var details = await _payrollDetailRepository.GetByPayrollRunIdAsync(payrollRunId);

        // Bangladesh Bank format (simplified)
        var csv = new System.Text.StringBuilder();
        csv.AppendLine("AccountNo,BankName,Amount,EmployeeName,EmployeeId");
        foreach (var detail in details)
        {
            csv.AppendLine($"{detail.BankAccountNo},{detail.BankName},{detail.NetSalary},{detail.Employee?.FirstName} {detail.Employee?.LastName},{detail.Employee?.EmployeeCode}");
        }

        return System.Text.Encoding.UTF8.GetBytes(csv.ToString());
    }
}

/// <summary>
/// Document service implementation
/// </summary>
public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IWebHostEnvironment _environment;

    public DocumentService(IDocumentRepository documentRepository, IWebHostEnvironment environment)
    {
        _documentRepository = documentRepository;
        _environment = environment;
    }

    public async Task<IEnumerable<HRPayroll.Domain.Entities.HR.EmployeeDocument>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _documentRepository.GetByEmployeeIdAsync(employeeId);
    }

    public async Task<HRPayroll.Domain.Entities.HR.EmployeeDocument?> GetDocumentByIdAsync(long id)
    {
        return await _documentRepository.GetByIdAsync(id);
    }

    public async Task<HRPayroll.Domain.Entities.HR.EmployeeDocument> UploadDocumentAsync(HRPayroll.Domain.Entities.HR.EmployeeDocument document)
    {
        return await _documentRepository.AddAsync(document);
    }

    public async Task<bool> DeleteDocumentAsync(long id)
    {
        var document = await _documentRepository.GetByIdAsync(id);
        if (document == null)
        {
            throw new Exception("Document not found.");
        }

        // Delete file from disk
        var filePath = Path.Combine(_environment.ContentRootPath, document.FilePath);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        await _documentRepository.DeleteAsync(document);
        return true;
    }

    public async Task<string> GetDocumentPathAsync(long documentId)
    {
        var document = await _documentRepository.GetByIdAsync(documentId);
        if (document == null)
        {
            throw new Exception("Document not found.");
        }

        return document.FilePath;
    }
}

/// <summary>
/// ESS (Employee Self-Service) service implementation
/// </summary>
public class ESSService : IESSService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly ILeaveRepository _leaveRepository;
    private readonly ILeaveBalanceRepository _leaveBalanceRepository;
    private readonly IPayrollDetailRepository _payrollDetailRepository;

    public ESSService(
        IEmployeeRepository employeeRepository,
        IAttendanceRepository attendanceRepository,
        ILeaveRepository leaveRepository,
        ILeaveBalanceRepository leaveBalanceRepository,
        IPayrollDetailRepository payrollDetailRepository)
    {
        _employeeRepository = employeeRepository;
        _attendanceRepository = attendanceRepository;
        _leaveRepository = leaveRepository;
        _leaveBalanceRepository = leaveBalanceRepository;
        _payrollDetailRepository = payrollDetailRepository;
    }

    public async Task<Employee?> GetEmployeeProfileAsync(long employeeId)
    {
        return await _employeeRepository.GetByIdAsync(employeeId);
    }

    public async Task<IEnumerable<Attendance>> GetMyAttendanceAsync(long employeeId, int month, int year)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        return await _attendanceRepository.GetByDateRangeAsync(employeeId, startDate, endDate);
    }

    public async Task<IEnumerable<Leave>> GetMyLeaveRequestsAsync(long employeeId)
    {
        return await _leaveRepository.GetByEmployeeIdAsync(employeeId);
    }

    public async Task<LeaveBalance[]> GetMyLeaveBalancesAsync(long employeeId, int year)
    {
        var balances = await _leaveBalanceRepository.GetByEmployeeIdAndYearAsync(employeeId, year);
        return balances.ToArray();
    }

    public async Task<byte[]> GetMyPayslipAsync(long employeeId, int month, int year)
    {
        // Simplified - should generate PDF
        var details = await _payrollDetailRepository.GetByEmployeeIdAsync(employeeId);
        var detail = details.FirstOrDefault(d => d.CreatedAt.Year == year && d.CreatedAt.Month == month);

        if (detail == null)
        {
            throw new Exception("Payslip not found.");
        }

        return System.Text.Encoding.UTF8.GetBytes($"Payslip for {month}/{year}: Net Salary = {detail.NetSalary}");
    }

    public async Task<byte[]> GetMyTaxCertificateAsync(long employeeId, int year)
    {
        // Simplified - should generate PDF
        return System.Text.Encoding.UTF8.GetBytes($"Tax Certificate for year {year}");
    }
}
