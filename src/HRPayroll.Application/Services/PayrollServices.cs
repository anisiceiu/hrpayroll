using HRPayroll.Domain.Dtos;
using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Entities.Payroll;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Interfaces;

namespace HRPayroll.Application.Services;

/// <summary>
/// Leave service implementation
/// </summary>
public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _leaveRepository;
    private readonly ILeaveBalanceRepository _leaveBalanceRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public LeaveService(
        ILeaveRepository leaveRepository,
        ILeaveBalanceRepository leaveBalanceRepository,
        ILeaveTypeRepository leaveTypeRepository,
        IEmployeeRepository employeeRepository)
    {
        _leaveRepository = leaveRepository;
        _leaveBalanceRepository = leaveBalanceRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<Leave>> GetAllLeavesAsync()
    {
        return await _leaveRepository.GetAllAsync();
    }

    public async Task<Leave?> GetLeaveByIdAsync(long id)
    {
        return await _leaveRepository.GetByIdAsync(id);
    }

    public async Task<Leave> ApplyLeaveAsync(Leave leave)
    {
        // Calculate total days
        leave.TotalDays = (leave.EndDate - leave.StartDate).Days + 1;
        if (leave.IsHalfDay)
        {
            leave.TotalDays = 0.5m;
        }

        // Check for overlapping leave requests
        var overlapping = await _leaveRepository.GetByEmployeeAndDateRangeAsync(
            leave.EmployeeId, leave.StartDate, leave.EndDate);
        if (overlapping != null)
        {
            throw new Exception("You already have a leave request for this date range.");
        }

        leave.Status = LeaveStatus.Pending;
        leave.AppliedOn = DateTime.Now;

        return await _leaveRepository.AddAsync(leave);
    }

    public async Task<Leave> UpdateLeaveAsync(Leave leave)
    {
        var existing = await _leaveRepository.GetByIdAsync(leave.Id);
        if (existing == null)
        {
            throw new Exception("Leave request not found.");
        }

        existing.LeaveTypeId = leave.LeaveTypeId;
        existing.StartDate = leave.StartDate;
        existing.EndDate = leave.EndDate;
        existing.TotalDays = leave.TotalDays;
        existing.Reason = leave.Reason;
        existing.IsHalfDay = leave.IsHalfDay;
        existing.HalfDayPortion = leave.HalfDayPortion;

        return await _leaveRepository.UpdateAsync(existing);
    }

    public async Task<bool> ApproveLeaveAsync(long leaveId, long approverId, string? remarks)
    {
        var leave = await _leaveRepository.GetByIdAsync(leaveId);
        if (leave == null)
        {
            throw new Exception("Leave request not found.");
        }

        if (leave.Status != LeaveStatus.Pending)
        {
            throw new Exception("Only pending leave requests can be approved.");
        }

        leave.Status = LeaveStatus.Approved;
        leave.ApprovedBy = approverId;
        leave.ApprovalDate = DateTime.Now;
        leave.ApprovalRemarks = remarks;

        // Update leave balance
        await UpdateLeaveBalanceAsync(leave);

        await _leaveRepository.UpdateAsync(leave);
        return true;
    }

    public async Task<bool> RejectLeaveAsync(long leaveId, long approverId, string? remarks)
    {
        var leave = await _leaveRepository.GetByIdAsync(leaveId);
        if (leave == null)
        {
            throw new Exception("Leave request not found.");
        }

        if (leave.Status != LeaveStatus.Pending)
        {
            throw new Exception("Only pending leave requests can be rejected.");
        }

        leave.Status = LeaveStatus.Rejected;
        leave.ApprovedBy = approverId;
        leave.ApprovalDate = DateTime.Now;
        leave.ApprovalRemarks = remarks;

        await _leaveRepository.UpdateAsync(leave);
        return true;
    }

    public async Task<bool> CancelLeaveAsync(long leaveId, long employeeId, string? reason)
    {
        var leave = await _leaveRepository.GetByIdAsync(leaveId);
        if (leave == null)
        {
            throw new Exception("Leave request not found.");
        }

        if (leave.EmployeeId != employeeId)
        {
            throw new Exception("You can only cancel your own leave requests.");
        }

        if (leave.Status != LeaveStatus.Pending && leave.Status != LeaveStatus.Approved)
        {
            throw new Exception("Only pending or approved leaves can be cancelled.");
        }

        leave.Status = LeaveStatus.Cancelled;
        leave.CancelledBy = employeeId;
        leave.CancelledDate = DateTime.Now;
        leave.CancellationReason = reason;

        // If approved, reverse the leave balance
        if (leave.Status == LeaveStatus.Approved)
        {
            await ReverseLeaveBalanceAsync(leave);
        }

        await _leaveRepository.UpdateAsync(leave);
        return true;
    }

    public async Task<IEnumerable<Leave>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _leaveRepository.GetByEmployeeIdAsync(employeeId);
    }

    public async Task<IEnumerable<Leave>> GetPendingApprovalsAsync(long approverId)
    {
        return await _leaveRepository.GetPendingApprovalsAsync(approverId);
    }

    public async Task<int> GetPendingCountAsync()
    {
        return await _leaveRepository.GetPendingCountAsync();
    }

    public async Task<IEnumerable<LeaveBalance>> GetLeaveBalancesAsync(long employeeId, int year)
    {
        return await _leaveBalanceRepository.GetByEmployeeIdAndYearAsync(employeeId, year);
    }

    public async Task<bool> UpdateLeaveBalanceAsync(long employeeId, long leaveTypeId, int year, decimal days)
    {
        var balance = await _leaveBalanceRepository.GetByEmployeeAndLeaveTypeAndYearAsync(employeeId, leaveTypeId, year);

        if (balance == null)
        {
            balance = new LeaveBalance
            {
                EmployeeId = employeeId,
                LeaveTypeId = leaveTypeId,
                Year = year,
                TotalDays = 0,
                UsedDays = days
            };
            await _leaveBalanceRepository.AddAsync(balance);
        }
        else
        {
            balance.UsedDays += days;
            await _leaveBalanceRepository.UpdateAsync(balance);
        }

        return true;
    }

    private async Task UpdateLeaveBalanceAsync(Leave leave)
    {
        var year = leave.StartDate.Year;
        await UpdateLeaveBalanceAsync(leave.EmployeeId, leave.LeaveTypeId, year, leave.TotalDays);
    }

    private async Task ReverseLeaveBalanceAsync(Leave leave)
    {
        var year = leave.StartDate.Year;
        var balance = await _leaveBalanceRepository.GetByEmployeeAndLeaveTypeAndYearAsync(
            leave.EmployeeId, leave.LeaveTypeId, year);

        if (balance != null)
        {
            balance.UsedDays -= leave.TotalDays;
            if (balance.UsedDays < 0) balance.UsedDays = 0;
            await _leaveBalanceRepository.UpdateAsync(balance);
        }
    }
}

/// <summary>
/// Payroll service implementation
/// </summary>
public class PayrollService : IPayrollService
{
    private readonly IPayrollRunRepository _payrollRunRepository;
    private readonly IPayrollDetailRepository _payrollDetailRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITaxConfigRepository _taxConfigRepository;
    private readonly AttendanceCalculationHelper _attendanceHelper;

    public PayrollService(
        IPayrollRunRepository payrollRunRepository,
        IPayrollDetailRepository payrollDetailRepository,
        IEmployeeRepository employeeRepository,
        ITaxConfigRepository taxConfigRepository,
        AttendanceCalculationHelper attendanceHelper)
    {
        _payrollRunRepository = payrollRunRepository;
        _payrollDetailRepository = payrollDetailRepository;
        _employeeRepository = employeeRepository;
        _taxConfigRepository = taxConfigRepository;
        _attendanceHelper = attendanceHelper;
    }

    public async Task<IEnumerable<PayrollRun>> GetAllPayrollRunsAsync()
    {
        return await _payrollRunRepository.GetAllAsync();
    }

    public async Task<PayrollRun?> GetPayrollRunByIdAsync(long id)
    {
        return await _payrollRunRepository.GetByIdAsync(id);
    }

    public async Task<PayrollRun> CreatePayrollRunAsync(PayrollRun payrollRun)
    {
        // Generate run code
        payrollRun.RunCode = $"PR-{payrollRun.Year}-{payrollRun.Month:D2}-001";

        // Check for existing payroll run for this month
        var existing = await _payrollRunRepository.GetByMonthAndYearAsync(payrollRun.Month, payrollRun.Year);
        if (existing != null)
        {
            payrollRun.RunCode = $"PR-{payrollRun.Year}-{payrollRun.Month:D2}-{(existing.Id + 1):D3}";
        }

        // Get eligible employees (active employees with salary structure)
        var employees = await _employeeRepository.GetActiveEmployeesWithSalaryStructureAsync();
        var eligibleEmployees = employees.ToList();

        // Calculate totals automatically
        payrollRun.TotalEmployees = eligibleEmployees.Count;
        payrollRun.TotalGrossSalary = eligibleEmployees
            .Sum(e => e.SalaryStructure?.GrossSalary ?? 0);
        payrollRun.TotalNetSalary = eligibleEmployees
            .Sum(e => e.SalaryStructure?.NetSalary ?? 0);
        payrollRun.TotalDeductions = eligibleEmployees
            .Sum(e => e.SalaryStructure?.TotalDeductions ?? 0);

        payrollRun.Status = PayrollRunStatus.Draft;
        return await _payrollRunRepository.AddAsync(payrollRun);
    }

    public async Task<PayrollRun> UpdatePayrollRunAsync(PayrollRun payrollRun)
    {
        var existing = await _payrollRunRepository.GetByIdAsync(payrollRun.Id);
        if (existing == null)
        {
            throw new Exception("Payroll run not found.");
        }

        existing.Month = payrollRun.Month;
        existing.Year = payrollRun.Year;
        existing.StartDate = payrollRun.StartDate;
        existing.EndDate = payrollRun.EndDate;
        existing.PaymentDate = payrollRun.PaymentDate;
        existing.Notes = payrollRun.Notes;

        return await _payrollRunRepository.UpdateAsync(existing);
    }

    /// <summary>
    /// Submit payroll run for approval (Draft -> PendingApproval)
    /// </summary>
    public async Task<bool> SubmitForApprovalAsync(long payrollRunId)
    {
        var payrollRun = await _payrollRunRepository.GetByIdAsync(payrollRunId);
        if (payrollRun == null)
        {
            throw new Exception("Payroll run not found.");
        }

        if (payrollRun.Status != PayrollRunStatus.Draft)
        {
            throw new Exception("Only draft payroll runs can be submitted for approval.");
        }

        payrollRun.Status = PayrollRunStatus.PendingApproval;
        //payrollRun.SubmittedDate = DateTime.Now;

        await _payrollRunRepository.UpdateAsync(payrollRun);
        return true;
    }

    public async Task<bool> ApprovePayrollRunAsync(long payrollRunId, long approverId)
    {
        var payrollRun = await _payrollRunRepository.GetByIdAsync(payrollRunId);
        if (payrollRun == null)
        {
            throw new Exception("Payroll run not found.");
        }

        if (payrollRun.Status != PayrollRunStatus.PendingApproval)
        {
            throw new Exception("Only pending payroll runs can be approved.");
        }

        payrollRun.Status = PayrollRunStatus.Approved;
        payrollRun.ApprovedById = (int?)approverId;
        payrollRun.ApprovedDate = DateTime.Now;

        await _payrollRunRepository.UpdateAsync(payrollRun);
        return true;
    }

    public async Task<bool> ProcessPayrollAsync(long payrollRunId)
    {
        var payrollRun = await _payrollRunRepository.GetByIdAsync(payrollRunId);
        if (payrollRun == null)
        {
            throw new Exception("Payroll run not found.");
        }

        if (payrollRun.Status != PayrollRunStatus.Approved)
        {
            throw new Exception("Only approved payroll runs can be processed.");
        }

        // Get date range from payroll run
        var startDate = payrollRun.StartDate ?? new DateTime(payrollRun.Year, payrollRun.Month, 1);
        var endDate = payrollRun.EndDate ?? startDate.AddMonths(1).AddDays(-1);

        // Get all active employees
        var employees = await _employeeRepository.GetActiveEmployeesAsync();

        foreach (var employee in employees)
        {
            if(employee.SalaryStructure is null)
                employee.SalaryStructure = new SalaryStructure();

            // Calculate working days and paid days using the helper
            var attendanceResult = await _attendanceHelper.CalculateAsync(
                employee.Id, startDate, endDate);

            var payrollDetail = new PayrollDetail
            {
                PayrollRunId = payrollRun.Id,
                EmployeeId = (int)employee.Id,
                BasicSalary = employee.SalaryStructure.BasicSalary, 
                GrossSalary = employee.SalaryStructure.GrossSalary,
                TotalEarnings = employee.SalaryStructure.BasicSalary +
                                employee.SalaryStructure.HouseRentAllowance +
                                employee.SalaryStructure.MedicalAllowance +
                                employee.SalaryStructure.TransportAllowance +
                                employee.SalaryStructure.OtherAllowances,
                TotalDeductions = employee.SalaryStructure.TotalDeductions,
                NetSalary = employee.SalaryStructure.NetSalary,
                TaxAmount = employee.SalaryStructure.TaxDeduction,
                WorkingDays = attendanceResult.WorkingDays,
                PaidDays = attendanceResult.PaidDays,
                BankAccountNo = employee.BankAccountNo,
                BankName = employee.BankName,
                PaymentStatus = PaymentStatus.Pending
            };

            await _payrollDetailRepository.AddAsync(payrollDetail);
        }

        payrollRun.Status = PayrollRunStatus.Processed;
        payrollRun.ProcessedById = 1; // Should get from current user
        payrollRun.ProcessedDate = DateTime.Now;

        await _payrollRunRepository.UpdateAsync(payrollRun);
        return true;
    }

    public async Task<bool> MarkAsPaidAsync(long payrollRunId)
    {
        var payrollRun = await _payrollRunRepository.GetByIdAsync(payrollRunId);
        if (payrollRun == null)
        {
            throw new Exception("Payroll run not found.");
        }

        if (payrollRun.Status != PayrollRunStatus.Processed)
        {
            throw new Exception("Only processed payroll runs can be marked as paid.");
        }

        payrollRun.Status = PayrollRunStatus.Paid;
        await _payrollRunRepository.UpdateAsync(payrollRun);
        return true;
    }

    public async Task<IEnumerable<PayrollDetail>> GetPayrollDetailsAsync(long payrollRunId)
    {
        return await _payrollDetailRepository.GetByPayrollRunIdAsync(payrollRunId);
    }

    public async Task<decimal> GetCurrentMonthCostAsync()
    {
        var currentMonth = DateTime.Now.Month;
        var currentYear = DateTime.Now.Year;
        var payrollRun = await _payrollRunRepository.GetByMonthAndYearAsync(currentMonth, currentYear);

        if (payrollRun == null) return 0;

        var details = await _payrollDetailRepository.GetByPayrollRunIdAsync(payrollRun.Id);
        return details.Sum(d => d.NetSalary);
    }

    public async Task<Dictionary<long, decimal>> GetCostByDepartmentAsync()
    {
        // Simplified - should join with employee department
        var payrollRuns = await _payrollRunRepository.GetAllAsync();
        var latestRun = payrollRuns.OrderByDescending(p => p.Year).ThenByDescending(p => p.Month).FirstOrDefault();

        if (latestRun == null) return new Dictionary<long, decimal>();

        var details = await _payrollDetailRepository.GetByPayrollRunIdAsync(latestRun.Id);
        return details.GroupBy(d => d.Employee.DepartmentId ?? 0)
            .ToDictionary(g => (long)g.Key, g => g.Sum(d => d.NetSalary));
    }

    public async Task<PayslipDto?> GetPayrollDetailByEmployeeAndMonthAsync(long employeeId, int month, int year)
    {
        // Get the payroll run for the specified month and year
        var payrollRun = await _payrollRunRepository.GetByMonthAndYearAsync(month, year);
        if (payrollRun == null)
        {
            return null;
        }

        // Get the payroll detail for the employee
        var payrollDetail = await _payrollDetailRepository.GetByEmployeeAndRunIdAsync(employeeId, payrollRun.Id);
        if (payrollDetail == null)
        {
            return null;
        }

        // Get the employee with related data
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
        {
            return null;
        }

        // Get salary structure for allowance details
        var salaryStructure = employee.SalaryStructure;

        // Calculate days present/absent (simplified - should get from attendance)
        var workingDays = payrollDetail.WorkingDays ?? 22;
        var daysPresent = payrollDetail.PaidDays ?? workingDays;
        var daysAbsent = workingDays - daysPresent;

        // Create and populate the PayslipDto
        var payslip = new PayslipDto
        {
            // Employee Information
            EmployeeId = employee.Id,
            EmployeeName = employee.FullName,
            EmployeeCode = employee.EmployeeCode,
            Department = employee.Department?.Name ?? "N/A",
            Designation = employee.Designation?.Name ?? "N/A",
            JoinDate = employee.JoiningDate,
            
            // Bank Details
            BankName = payrollDetail.BankName ?? employee.BankName ?? "N/A",
            BankAccountNo = payrollDetail.BankAccountNo ?? employee.BankAccountNo ?? "N/A",
            BranchName = employee.BranchName ?? "N/A",
            
            // Pay Period
            Month = month,
            Year = year,
            MonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
            WorkingDays = workingDays,
            DaysPresent = daysPresent,
            DaysAbsent = daysAbsent,
            
            // Earnings
            BasicSalary = payrollDetail.BasicSalary,
            HouseRentAllowance = salaryStructure?.HouseRentAllowance ?? 0,
            TransportAllowance = salaryStructure?.TransportAllowance ?? 0,
            MedicalAllowance = salaryStructure?.MedicalAllowance ?? 0,
            OvertimeAmount = payrollDetail.OvertimeAmount ?? 0,
            TotalEarnings = payrollDetail.TotalEarnings,
            
            // Deductions
            ProvidentFund = payrollDetail.ProvidentFund ?? 0,
            TaxDeduction = payrollDetail.TaxDeduction ?? payrollDetail.TaxAmount ?? 0,
            Insurance = 0, // Can be enhanced with insurance repository
            OtherDeductions = payrollDetail.OtherDeductions ?? 0,
            TotalDeductions = payrollDetail.TotalDeductions,
            
            // Summary
            GrossSalary = payrollDetail.GrossSalary,
            NetSalary = payrollDetail.NetSalary
        };

        return payslip;
    }
}
