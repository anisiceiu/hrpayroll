using HRPayroll.Domain.Dtos;

namespace HRPayroll.Domain.Interfaces;

/// <summary>
/// Employee service interface
/// </summary>
public interface IEmployeeService
{
    Task<IEnumerable<Entities.HR.Employee>> GetAllEmployeesAsync();
    Task<Entities.HR.Employee?> GetEmployeeByIdAsync(long id);
    Task<Entities.HR.Employee?> GetEmployeeByCodeAsync(string employeeCode);
    Task<Entities.HR.Employee> CreateEmployeeAsync(Entities.HR.Employee employee);
    Task<Entities.HR.Employee> UpdateEmployeeAsync(Entities.HR.Employee employee);
    Task<bool> DeleteEmployeeAsync(long id);
    Task<int> GetTotalCountAsync();
    Task<int> GetActiveCountAsync();
    Task<IEnumerable<Entities.HR.Employee>> GetByDepartmentIdAsync(long departmentId);
    Task<IEnumerable<Entities.HR.Employee>> GetActiveEmployeesAsync();
    // New methods with navigation properties populated
    Task<IEnumerable<Entities.HR.Employee>> GetAllEmployeesWithIncludesAsync();
    Task<Entities.HR.Employee?> GetEmployeeByIdWithIncludesAsync(long id);
    Task<IEnumerable<Entities.HR.Employee>> GetActiveEmployeesWithIncludesAsync();
}

/// <summary>
/// Department service interface
/// </summary>
public interface IDepartmentService
{
    Task<IEnumerable<Entities.HR.Department>> GetAllDepartmentsAsync();
    Task<Entities.HR.Department?> GetDepartmentByIdAsync(long id);
    Task<Entities.HR.Department> CreateDepartmentAsync(Entities.HR.Department department);
    Task<Entities.HR.Department> UpdateDepartmentAsync(Entities.HR.Department department);
    Task<bool> DeleteDepartmentAsync(long id);
    Task<IEnumerable<Entities.HR.Department>> GetActiveDepartmentsAsync();
}

/// <summary>
/// Designation service interface
/// </summary>
public interface IDesignationService
{
    Task<IEnumerable<Entities.HR.Designation>> GetAllDesignationsAsync();
    Task<Entities.HR.Designation?> GetDesignationByIdAsync(long id);
    Task<Entities.HR.Designation> CreateDesignationAsync(Entities.HR.Designation designation);
    Task<Entities.HR.Designation> UpdateDesignationAsync(Entities.HR.Designation designation);
    Task<bool> DeleteDesignationAsync(long id);
    Task<IEnumerable<Entities.HR.Designation>> GetByDepartmentIdAsync(long departmentId);
    Task<IEnumerable<Entities.HR.Designation>> GetActiveDesignationsAsync();
}

/// <summary>
/// Shift service interface
/// </summary>
public interface IShiftService
{
    Task<IEnumerable<Entities.HR.Shift>> GetAllShiftsAsync();
    Task<Entities.HR.Shift?> GetShiftByIdAsync(long id);
    Task<Entities.HR.Shift> CreateShiftAsync(Entities.HR.Shift shift);
    Task<Entities.HR.Shift> UpdateShiftAsync(Entities.HR.Shift shift);
    Task<bool> DeleteShiftAsync(long id);
    Task<IEnumerable<Entities.HR.Shift>> GetActiveShiftsAsync();
    // New methods with navigation properties populated
    Task<IEnumerable<Entities.HR.Shift>> GetAllShiftsWithIncludesAsync();
    Task<Entities.HR.Shift?> GetShiftByIdWithIncludesAsync(long id);
    Task<IEnumerable<Entities.HR.Shift>> GetActiveShiftsWithIncludesAsync();
}

/// <summary>
/// EmployeeShift service interface
/// </summary>
public interface IEmployeeShiftService
{
    Task<IEnumerable<Entities.HR.EmployeeShift>> GetAllEmployeeShiftsAsync();
    Task<Entities.HR.EmployeeShift?> GetEmployeeShiftByIdAsync(long id);
    Task<IEnumerable<Entities.HR.EmployeeShift>> GetByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.HR.EmployeeShift>> GetByShiftIdAsync(long shiftId);
    Task<Entities.HR.EmployeeShift?> GetCurrentAssignmentAsync(long employeeId);
    Task<Entities.HR.EmployeeShift> AssignShiftAsync(Entities.HR.EmployeeShift employeeShift);
    Task<Entities.HR.EmployeeShift> UpdateEmployeeShiftAsync(Entities.HR.EmployeeShift employeeShift);
    Task<bool> RemoveAssignmentAsync(long id);
    Task<IEnumerable<Entities.HR.EmployeeShift>> GetActiveAssignmentsAsync();
}

/// <summary>
/// Attendance service interface
/// </summary>
public interface IAttendanceService
{
    Task<IEnumerable<Entities.HR.Attendance>> GetAllAttendancesAsync();
    Task<Entities.HR.Attendance?> GetAttendanceByIdAsync(long id);
    Task<Entities.HR.Attendance?> GetByEmployeeAndDateAsync(long employeeId, DateTime date);
    Task<Entities.HR.Attendance> RecordAttendanceAsync(Entities.HR.Attendance attendance);
    Task<Entities.HR.Attendance> UpdateAttendanceAsync(Entities.HR.Attendance attendance);
    Task<bool> DeleteAttendanceAsync(long id);
    Task<IEnumerable<Entities.HR.Attendance>> GetByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.HR.Attendance>> GetByDateRangeAsync(long employeeId, DateTime startDate, DateTime endDate);
    Task<int> GetTodayPresentCountAsync();
    Task<int> GetTodayAbsentCountAsync();
    Task<int> GetTodayLateCountAsync();
    Task<bool> ImportFromBiometricAsync(IEnumerable<Entities.HR.Attendance> attendances);
    
    // Employee self-service clock in/out methods
    Task<Entities.HR.Attendance> ClockInAsync(long employeeId);
    Task<Entities.HR.Attendance?> ClockOutAsync(long employeeId);
    Task<Entities.HR.Attendance?> GetTodayAttendanceAsync(long employeeId);
}

/// <summary>
/// Leave service interface
/// </summary>
public interface ILeaveService
{
    Task<IEnumerable<Entities.HR.Leave>> GetAllLeavesAsync();
    Task<Entities.HR.Leave?> GetLeaveByIdAsync(long id);
    Task<Entities.HR.Leave> ApplyLeaveAsync(Entities.HR.Leave leave);
    Task<Entities.HR.Leave> UpdateLeaveAsync(Entities.HR.Leave leave);
    Task<bool> ApproveLeaveAsync(long leaveId, long approverId, string? remarks);
    Task<bool> RejectLeaveAsync(long leaveId, long approverId, string? remarks);
    Task<bool> CancelLeaveAsync(long leaveId, long employeeId, string? reason);
    Task<IEnumerable<Entities.HR.Leave>> GetByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.HR.Leave>> GetPendingApprovalsAsync(long approverId);
    Task<int> GetPendingCountAsync();
    Task<IEnumerable<Entities.HR.LeaveBalance>> GetLeaveBalancesAsync(long employeeId, int year);
    Task<bool> UpdateLeaveBalanceAsync(long employeeId, long leaveTypeId, int year, decimal days);
}

/// <summary>
/// LeaveType service interface
/// </summary>
public interface ILeaveTypeService
{
    Task<IEnumerable<Entities.HR.LeaveType>> GetAllLeaveTypesAsync();
    Task<Entities.HR.LeaveType?> GetLeaveTypeByIdAsync(long id);
    Task<Entities.HR.LeaveType> CreateLeaveTypeAsync(Entities.HR.LeaveType leaveType);
    Task<Entities.HR.LeaveType> UpdateLeaveTypeAsync(Entities.HR.LeaveType leaveType);
    Task<bool> DeleteLeaveTypeAsync(long id);
    Task<IEnumerable<Entities.HR.LeaveType>> GetActiveLeaveTypesAsync();
}

/// <summary>
/// Holiday service interface
/// </summary>
public interface IHolidayService
{
    Task<IEnumerable<Entities.HR.Holiday>> GetAllHolidaysAsync();
    Task<Entities.HR.Holiday?> GetHolidayByIdAsync(long id);
    Task<Entities.HR.Holiday> CreateHolidayAsync(Entities.HR.Holiday holiday);
    Task<Entities.HR.Holiday> UpdateHolidayAsync(Entities.HR.Holiday holiday);
    Task<bool> DeleteHolidayAsync(long id);
    Task<IEnumerable<Entities.HR.Holiday>> GetByYearAsync(int year);
    Task<IEnumerable<Entities.HR.Holiday>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}

/// <summary>
/// Payroll service interface
/// </summary>
public interface IPayrollService
{
    Task<IEnumerable<Entities.Payroll.PayrollRun>> GetAllPayrollRunsAsync();
    Task<Entities.Payroll.PayrollRun?> GetPayrollRunByIdAsync(long id);
    Task<Entities.Payroll.PayrollRun> CreatePayrollRunAsync(Entities.Payroll.PayrollRun payrollRun);
    Task<Entities.Payroll.PayrollRun> UpdatePayrollRunAsync(Entities.Payroll.PayrollRun payrollRun);
    Task<bool> ApprovePayrollRunAsync(long payrollRunId, long approverId);
    Task<bool> ProcessPayrollAsync(long payrollRunId);
    Task<bool> MarkAsPaidAsync(long payrollRunId);
    Task<IEnumerable<Entities.Payroll.PayrollDetail>> GetPayrollDetailsAsync(long payrollRunId);
    Task<decimal> GetCurrentMonthCostAsync();
    Task<Dictionary<long, decimal>> GetCostByDepartmentAsync();
    Task<PayslipDto?> GetPayrollDetailByEmployeeAndMonthAsync(long employeeId, int month, int year);
    // New method to submit for approval
    Task<bool> SubmitForApprovalAsync(long payrollRunId);
}

/// <summary>
/// Tax service interface
/// </summary>
public interface ITaxService
{
    Task<decimal> CalculateMonthlyTaxAsync(long employeeId, int month, int year);
    Task<decimal> CalculateAnnualTaxAsync(long employeeId, int year);
    Task<Entities.Payroll.TaxConfig?> GetCurrentTaxConfigAsync();
    Task<Entities.Payroll.TaxConfig> CreateTaxConfigAsync(Entities.Payroll.TaxConfig taxConfig);
    Task<Entities.Payroll.TaxConfig> UpdateTaxConfigAsync(Entities.Payroll.TaxConfig taxConfig);
    Task<IEnumerable<Entities.Payroll.TaxSlab>> GetCurrentTaxSlabsAsync();
}

/// <summary>
/// Report service interface
/// </summary>
public interface IReportService
{
    Task<byte[]> GenerateEmployeeReportAsync(long? departmentId = null);
    Task<byte[]> GenerateAttendanceReportAsync(DateTime startDate, DateTime endDate, long? departmentId = null);
    Task<byte[]> GeneratePayrollSummaryAsync(int month, int year);
    Task<byte[]> GenerateTaxReportAsync(int year);
    Task<byte[]> GenerateBankTransferFileAsync(long payrollRunId);
}

/// <summary>
/// Document service interface
/// </summary>
public interface IDocumentService
{
    Task<IEnumerable<Entities.HR.EmployeeDocument>> GetByEmployeeIdAsync(long employeeId);
    Task<Entities.HR.EmployeeDocument?> GetDocumentByIdAsync(long id);
    Task<Entities.HR.EmployeeDocument> UploadDocumentAsync(Entities.HR.EmployeeDocument document);
    Task<Entities.HR.EmployeeDocument> UpdateDocumentAsync(Entities.HR.EmployeeDocument document);
    Task<bool> DeleteDocumentAsync(long id);
    Task<string> GetDocumentPathAsync(long documentId);
    Task<byte[]> DownloadDocumentAsync(long documentId);
    Task<Entities.HR.EmployeeDocument> ReplaceDocumentAsync(long documentId, Entities.HR.EmployeeDocument newDocument);
    Task<IEnumerable<Entities.HR.EmployeeDocument>> GetByCategoryIdAsync(long categoryId);
    Task<IEnumerable<Entities.HR.EmployeeDocument>> GetAllDocumentsWithIncludesAsync();
}

/// <summary>
/// DocumentCategory service interface
/// </summary>
public interface IDocumentCategoryService
{
    Task<IEnumerable<Entities.HR.DocumentCategory>> GetAllCategoriesAsync();
    Task<Entities.HR.DocumentCategory?> GetCategoryByIdAsync(long id);
    Task<Entities.HR.DocumentCategory> CreateCategoryAsync(Entities.HR.DocumentCategory category);
    Task<Entities.HR.DocumentCategory> UpdateCategoryAsync(Entities.HR.DocumentCategory category);
    Task<bool> DeleteCategoryAsync(long id);
    Task<IEnumerable<Entities.HR.DocumentCategory>> GetActiveCategoriesAsync();
}

/// <summary>
/// SalaryStructure service interface
/// </summary>
public interface ISalaryStructureService
{
    Task<IEnumerable<Entities.Payroll.SalaryStructure>> GetAllSalaryStructuresAsync();
    Task<Entities.Payroll.SalaryStructure?> GetSalaryStructureByIdAsync(long id);
    Task<Entities.Payroll.SalaryStructure?> GetByEmployeeIdAsync(long employeeId);
    Task<Entities.Payroll.SalaryStructure> CreateSalaryStructureAsync(Entities.Payroll.SalaryStructure salaryStructure);
    Task<Entities.Payroll.SalaryStructure> UpdateSalaryStructureAsync(Entities.Payroll.SalaryStructure salaryStructure);
    Task<bool> DeleteSalaryStructureAsync(long id);
    Task<IEnumerable<Entities.Payroll.SalaryStructure>> GetActiveStructuresAsync();
    // New methods with navigation properties populated
    Task<IEnumerable<Entities.Payroll.SalaryStructure>> GetAllSalaryStructuresWithIncludesAsync();
    Task<Entities.Payroll.SalaryStructure?> GetSalaryStructureByIdWithIncludesAsync(long id);
    Task<Entities.Payroll.SalaryStructure?> GetByEmployeeIdWithIncludesAsync(long employeeId);
    Task<IEnumerable<Entities.Payroll.SalaryStructure>> GetActiveStructuresWithIncludesAsync();
}

/// <summary>
/// ESS (Employee Self-Service) interface
/// </summary>
public interface IESSService
{
    Task<Entities.HR.Employee?> GetEmployeeProfileAsync(long employeeId);
    Task<IEnumerable<Entities.HR.Attendance>> GetMyAttendanceAsync(long employeeId, int month, int year);
    Task<IEnumerable<Entities.HR.Leave>> GetMyLeaveRequestsAsync(long employeeId);
    Task<Entities.HR.LeaveBalance[]> GetMyLeaveBalancesAsync(long employeeId, int year);
    Task<byte[]> GetMyPayslipAsync(long employeeId, int month, int year);
    Task<byte[]> GetMyTaxCertificateAsync(long employeeId, int year);
}
