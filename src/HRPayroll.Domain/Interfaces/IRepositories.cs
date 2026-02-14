using HRPayroll.Domain.Common;
using HRPayroll.Domain.Entities.HR;

namespace HRPayroll.Domain.Interfaces;

/// <summary>
/// Generic repository interface
/// </summary>
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(long id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllActiveAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
    Task<int> SaveChangesAsync();
}

/// <summary>
/// Employee repository interface
/// </summary>
public interface IEmployeeRepository : IRepository<Entities.HR.Employee>
{
    Task<Entities.HR.Employee?> GetByEmployeeCodeAsync(string employeeCode);
    Task<Entities.HR.Employee?> GetByEmailAsync(string email);
    Task<IEnumerable<Entities.HR.Employee>> GetByDepartmentIdAsync(long departmentId);
    Task<IEnumerable<Entities.HR.Employee>> GetByStatusAsync(Enums.EmployeeStatus status);
    Task<IEnumerable<Entities.HR.Employee>> GetActiveEmployeesAsync();
    Task<IEnumerable<Entities.HR.Employee>> GetAllWithIncludesAsync();
    Task<Entities.HR.Employee?> GetByIdWithIncludesAsync(long id);
    Task<IEnumerable<Entities.HR.Employee>> GetActiveEmployeesWithIncludesAsync();
    Task<IEnumerable<Entities.HR.Employee>> GetActiveEmployeesWithSalaryStructureAsync();
    Task<int> GetTotalCountAsync();
    Task<int> GetActiveCountAsync();
    
}

/// <summary>
/// Department repository interface
/// </summary>
public interface IDepartmentRepository : IRepository<Entities.HR.Department>
{
    Task<Entities.HR.Department?> GetByCodeAsync(string code);
    Task<IEnumerable<Entities.HR.Department>> GetActiveDepartmentsAsync();
    Task<IEnumerable<Entities.HR.Department>> GetSubDepartmentsAsync(long parentId);
    Task<IEnumerable<Entities.HR.Department>> GetAllWithIncludesAsync();
    Task<int> GetDepartmentCountAsync();
}

/// <summary>
/// Designation repository interface
/// </summary>
public interface IDesignationRepository : IRepository<Entities.HR.Designation>
{
    Task<Entities.HR.Designation?> GetByCodeAsync(string code);
    Task<IEnumerable<Entities.HR.Designation>> GetByDepartmentIdAsync(long departmentId);
    Task<IEnumerable<Entities.HR.Designation>> GetActiveDesignationsAsync();
    Task<IEnumerable<Entities.HR.Designation>> GetAllWithIncludesAsync();
    Task<int> GetDesignationCountAsync();
}

/// <summary>
/// Shift repository interface
/// </summary>
public interface IShiftRepository : IRepository<Entities.HR.Shift>
{
    Task<Entities.HR.Shift?> GetByCodeAsync(string code);
    Task<IEnumerable<Entities.HR.Shift>> GetActiveShiftsAsync();
    // New methods with navigation properties populated
    Task<IEnumerable<Entities.HR.Shift>> GetAllWithIncludesAsync();
    Task<Entities.HR.Shift?> GetByIdWithIncludesAsync(long id);
    Task<IEnumerable<Entities.HR.Shift>> GetActiveShiftsWithIncludesAsync();
    Task<int> GetShiftCountAsync();
}

/// <summary>
/// EmployeeShift repository interface
/// </summary>
public interface IEmployeeShiftRepository : IRepository<Entities.HR.EmployeeShift>
{
    Task<EmployeeShift?> GetCurrentAssignmentWithIncludesAsync(long employeeId);
    Task<IEnumerable<EmployeeShift>> GetByEmployeeIdWithIncludesAsync(long employeeId);
    Task<IEnumerable<Entities.HR.EmployeeShift>> GetAllWithIncludesAsync();
    Task<IEnumerable<Entities.HR.EmployeeShift>> GetByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.HR.EmployeeShift>> GetByShiftIdAsync(long shiftId);
    Task<Entities.HR.EmployeeShift?> GetCurrentAssignmentAsync(long employeeId);
    Task<IEnumerable<Entities.HR.EmployeeShift>> GetActiveAssignmentsAsync();
    Task<IEnumerable<Entities.HR.EmployeeShift>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}

/// <summary>
/// Attendance repository interface
/// </summary>
public interface IAttendanceRepository : IRepository<Entities.HR.Attendance>
{
    Task<IEnumerable<Entities.HR.Attendance>> GetAllWithIncludeAsync();
    Task<Entities.HR.Attendance?> GetByEmployeeAndDateAsync(long employeeId, DateTime date);
    Task<IEnumerable<Entities.HR.Attendance>> GetByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.HR.Attendance>> GetByDateRangeAsync(long employeeId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Entities.HR.Attendance>> GetByDateAsync(DateTime date);
    Task<int> GetTodayPresentCountAsync();
    Task<int> GetTodayAbsentCountAsync();
    Task<int> GetTodayLateCountAsync();
}

/// <summary>
/// LeaveType repository interface
/// </summary>
public interface ILeaveTypeRepository : IRepository<HRPayroll.Domain.Entities.HR.LeaveType>
{
    Task<HRPayroll.Domain.Entities.HR.LeaveType?> GetByCodeAsync(string code);
    Task<IEnumerable<HRPayroll.Domain.Entities.HR.LeaveType>> GetActiveLeaveTypesAsync();
}

/// <summary>
/// Leave repository interface
/// </summary>
public interface ILeaveRepository : IRepository<Entities.HR.Leave>
{
    Task<Entities.HR.Leave?> GetByEmployeeAndDateRangeAsync(long employeeId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Entities.HR.Leave>> GetByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.HR.Leave>> GetByStatusAsync(Enums.LeaveStatus status);
    Task<IEnumerable<Entities.HR.Leave>> GetPendingApprovalsAsync(long approverId);
    Task<int> GetPendingCountAsync();
}

/// <summary>
/// LeaveBalance repository interface
/// </summary>
public interface ILeaveBalanceRepository : IRepository<Entities.HR.LeaveBalance>
{
    Task<Entities.HR.LeaveBalance?> GetByEmployeeAndLeaveTypeAndYearAsync(long employeeId, long leaveTypeId, int year);
    Task<IEnumerable<Entities.HR.LeaveBalance>> GetByEmployeeIdAndYearAsync(long employeeId, int year);
}

/// <summary>
/// Holiday repository interface
/// </summary>
public interface IHolidayRepository : IRepository<Entities.HR.Holiday>
{
    Task<IEnumerable<Entities.HR.Holiday>> GetByYearAsync(int year);
    Task<IEnumerable<Entities.HR.Holiday>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Entities.HR.Holiday>> GetNationalHolidaysAsync();
}

/// <summary>
/// Document repository interface
/// </summary>
public interface IDocumentRepository : IRepository<Entities.HR.EmployeeDocument>
{
    Task<IEnumerable<Entities.HR.EmployeeDocument>> GetByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.HR.EmployeeDocument>> GetByTypeAsync(string documentType);
    Task<IEnumerable<Entities.HR.EmployeeDocument>> GetByEmployeeIdWithIncludesAsync(long employeeId);
    Task<IEnumerable<Entities.HR.EmployeeDocument>> GetByCategoryIdAsync(long categoryId);
    Task<IEnumerable<Entities.HR.EmployeeDocument>> GetAllWithIncludesAsync();
}

/// <summary>
/// DocumentCategory repository interface
/// </summary>
public interface IDocumentCategoryRepository : IRepository<Entities.HR.DocumentCategory>
{
    Task<Entities.HR.DocumentCategory?> GetByCodeAsync(string code);
    Task<IEnumerable<Entities.HR.DocumentCategory>> GetActiveCategoriesAsync();
}

/// <summary>
/// SalaryStructure repository interface
/// </summary>
public interface ISalaryStructureRepository : IRepository<Entities.Payroll.SalaryStructure>
{
    Task<Entities.Payroll.SalaryStructure?> GetByCodeAsync(string code);
    Task<Entities.Payroll.SalaryStructure?> GetByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.Payroll.SalaryStructure>> GetActiveStructuresAsync();
    // New methods with navigation properties populated
    Task<IEnumerable<Entities.Payroll.SalaryStructure>> GetAllWithIncludesAsync();
    Task<Entities.Payroll.SalaryStructure?> GetByIdWithIncludesAsync(long id);
    Task<Entities.Payroll.SalaryStructure?> GetByEmployeeIdWithIncludesAsync(long employeeId);
    Task<IEnumerable<Entities.Payroll.SalaryStructure>> GetActiveStructuresWithIncludesAsync();
}

/// <summary>
/// SalaryComponent repository interface
/// </summary>
public interface ISalaryComponentRepository : IRepository<Entities.Payroll.SalaryComponent>
{
    Task<IEnumerable<Entities.Payroll.SalaryComponent>> GetByStructureIdAsync(long structureId);
    // New methods with navigation properties populated
    Task<IEnumerable<Entities.Payroll.SalaryComponent>> GetAllWithIncludesAsync();
    Task<IEnumerable<Entities.Payroll.SalaryComponent>> GetByStructureIdWithIncludesAsync(long structureId);
}

/// <summary>
/// PayrollRun repository interface
/// </summary>
public interface IPayrollRunRepository : IRepository<Entities.Payroll.PayrollRun>
{
    Task<Entities.Payroll.PayrollRun?> GetByRunCodeAsync(string runCode);
    Task<Entities.Payroll.PayrollRun?> GetByMonthAndYearAsync(int month, int year);
    Task<IEnumerable<Entities.Payroll.PayrollRun>> GetByYearAsync(int year);
    Task<IEnumerable<Entities.Payroll.PayrollRun>> GetPendingApprovalAsync();
}

/// <summary>
/// PayrollDetail repository interface
/// </summary>
public interface IPayrollDetailRepository : IRepository<Entities.Payroll.PayrollDetail>
{
    Task<IEnumerable<Entities.Payroll.PayrollDetail>> GetByPayrollRunIdAsync(long payrollRunId);
    Task<IEnumerable<Entities.Payroll.PayrollDetail>> GetByEmployeeIdAsync(long employeeId);
    Task<Entities.Payroll.PayrollDetail?> GetByEmployeeAndRunIdAsync(long employeeId, long payrollRunId);
}

/// <summary>
/// Loan repository interface
/// </summary>
public interface ILoanRepository : IRepository<Entities.Payroll.Loan>
{
    Task<IEnumerable<Entities.Payroll.Loan>> GetByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.Payroll.Loan>> GetActiveLoansByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.Payroll.Loan>> GetByStatusAsync(Enums.LoanStatus status);
}

/// <summary>
/// Bonus repository interface
/// </summary>
public interface IBonusRepository : IRepository<Entities.Payroll.Bonus>
{
    Task<IEnumerable<Entities.Payroll.Bonus>> GetByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.Payroll.Bonus>> GetByPayrollRunIdAsync(long payrollRunId);
    Task<IEnumerable<Entities.Payroll.Bonus>> GetByBonusTypeAsync(Enums.BonusType bonusType);
}

/// <summary>
/// Overtime repository interface
/// </summary>
public interface IOvertimeRepository : IRepository<Entities.Payroll.Overtime>
{
    Task<IEnumerable<Entities.Payroll.Overtime>> GetByEmployeeIdAsync(long employeeId);
    Task<IEnumerable<Entities.Payroll.Overtime>> GetByDateRangeAsync(long employeeId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Entities.Payroll.Overtime>> GetPendingApprovalAsync(long approverId);
    Task<IEnumerable<Entities.Payroll.Overtime>> GetApprovedNotPaidAsync();
}

/// <summary>
/// TaxConfig repository interface
/// </summary>
public interface ITaxConfigRepository : IRepository<Entities.Payroll.TaxConfig>
{
    Task<Entities.Payroll.TaxConfig?> GetByTaxYearAsync(int taxYear);
    Task<Entities.Payroll.TaxConfig?> GetCurrentConfigAsync();
}

/// <summary>
/// TaxSlab repository interface
/// </summary>
public interface ITaxSlabRepository : IRepository<Entities.Payroll.TaxSlab>
{
    Task<IEnumerable<Entities.Payroll.TaxSlab>> GetByConfigIdAsync(long configId);
    Task<IEnumerable<Entities.Payroll.TaxSlab>> GetCurrentTaxSlabsAsync();
}

/// <summary>
/// Unit of Work interface
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
