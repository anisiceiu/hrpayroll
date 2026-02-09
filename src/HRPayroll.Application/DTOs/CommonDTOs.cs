using System.ComponentModel.DataAnnotations;
using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Entities.Payroll;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Application.DTOs;

// Department DTOs
public class DepartmentDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameBN { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long? ParentDepartmentId { get; set; }
    public long? HeadOfDepartmentId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class DepartmentListDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? ParentDepartmentName { get; set; }
    public string? HeadOfDepartmentName { get; set; }
    public int EmployeeCount { get; set; }
    public bool IsActive { get; set; }
}

// Designation DTOs
public class DesignationDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameBN { get; set; }
    public string Code { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string? Description { get; set; }
    public string? Grade { get; set; }
    public string? Level { get; set; }
    public decimal? BaseSalary { get; set; }
    public bool IsActive { get; set; } = true;
}

public class DesignationListDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string? Grade { get; set; }
    public int EmployeeCount { get; set; }
    public bool IsActive { get; set; }
}

// Shift DTOs
public class ShiftDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int BreakTimeMinutes { get; set; } = 60;
    public decimal WorkingHours { get; set; }
    public int GraceTimeMinutes { get; set; }
    public bool IsNightShift { get; set; }
    public bool IsActive { get; set; } = true;
}

public class ShiftListDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public decimal WorkingHours { get; set; }
    public int EmployeeCount { get; set; }
    public bool IsActive { get; set; }
}

// Attendance DTOs
public class AttendanceDTO
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan? ClockInTime { get; set; }
    public TimeSpan? ClockOutTime { get; set; }
    public decimal? WorkingHours { get; set; }
    public decimal? OvertimeHours { get; set; }
    public AttendanceStatus Status { get; set; }
    public int? LateMinutes { get; set; }
    public EntryType EntryType { get; set; } = EntryType.Manual;
    public string? Remarks { get; set; }
}

public class AttendanceListDTO
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeCode { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? ClockInTime { get; set; }
    public string? ClockOutTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? WorkingHours { get; set; }
}

// Leave DTOs
public class LeaveTypeDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameBN { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPaidLeave { get; set; }
    public bool IsCarryForwardAllowed { get; set; }
    public int? MaxCarryForwardDays { get; set; }
    public bool RequiresApproval { get; set; }
    public string? ColorCode { get; set; }
    public bool IsActive { get; set; }
}

public class LeaveDTO
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public long LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalDays { get; set; }
    public string? Reason { get; set; }
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
    public bool IsHalfDay { get; set; }
    public HalfDayPortion? HalfDayPortion { get; set; }
    public string? SupportingDocument { get; set; }
}

public class LeaveListDTO
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public long LeaveTypeId { get; set; }
    public string LeaveTypeName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalDays { get; set; }
    public string? Reason { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime AppliedOn { get; set; }
}

public class LeaveBalanceDTO
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public long LeaveTypeId { get; set; }
    public string LeaveTypeName { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal TotalDays { get; set; }
    public decimal UsedDays { get; set; }
    public decimal PendingDays { get; set; }
    public decimal CarryForwardDays { get; set; }
    public decimal AvailableDays { get; set; }
}

// Holiday DTOs
public class HolidayDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameBN { get; set; }
    public DateTime Date { get; set; }
    public string? DayOfWeek { get; set; }
    public HolidayType Type { get; set; }
    public string? Description { get; set; }
    public bool IsRepeatAnnually { get; set; }
}

public class HolidayListDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? DayOfWeek { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Description { get; set; }
}

// Payroll DTOs
public class PayrollRunDTO
{
    public long Id { get; set; }
    public string RunCode { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public PayrollRunStatus Status { get; set; } = PayrollRunStatus.Draft;
    public string? Notes { get; set; }
}

public class PayrollRunListDTO
{
    public long Id { get; set; }
    public string RunCode { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public string Status { get; set; } = string.Empty;
    public int TotalEmployees { get; set; }
    public decimal? GrossSalary { get; set; }
    public decimal? NetSalary { get; set; }
    public DateTime? PaymentDate { get; set; }
}

public class PayrollDetailDTO
{
    public long Id { get; set; }
    public long PayrollRunId { get; set; }
    public long EmployeeId { get; set; }
    public decimal? BasicSalary { get; set; }
    public decimal? GrossSalary { get; set; }
    public decimal? TotalEarnings { get; set; }
    public decimal? TotalDeductions { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? TaxAmount { get; set; }
    public int? WorkingDays { get; set; }
    public int? PaidDays { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
}

public class PayrollDetailListDTO
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public decimal? BasicSalary { get; set; }
    public decimal? GrossSalary { get; set; }
    public decimal? TotalDeductions { get; set; }
    public decimal? NetSalary { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
}
