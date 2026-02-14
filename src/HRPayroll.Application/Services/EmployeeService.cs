using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Interfaces;

namespace HRPayroll.Application.Services;

/// <summary>
/// Employee service implementation
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await _employeeRepository.GetAllAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(long id)
    {
        return await _employeeRepository.GetByIdAsync(id);
    }

    public async Task<Employee?> GetEmployeeByCodeAsync(string employeeCode)
    {
        return await _employeeRepository.GetByEmployeeCodeAsync(employeeCode);
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        // Generate employee code if not provided
        if (string.IsNullOrEmpty(employee.EmployeeCode))
        {
            employee.EmployeeCode = await GenerateEmployeeCodeAsync();
        }

        // Check for duplicate email
        var existingEmployee = await _employeeRepository.GetByEmailAsync(employee.Email);
        if (existingEmployee != null)
        {
            throw new Exception("An employee with this email already exists.");
        }

        // Check for duplicate employee code
        existingEmployee = await _employeeRepository.GetByEmployeeCodeAsync(employee.EmployeeCode);
        if (existingEmployee != null)
        {
            throw new Exception("An employee with this code already exists.");
        }

        employee.Status = EmployeeStatus.Active;
        return await _employeeRepository.AddAsync(employee);
    }

    public async Task<Employee> UpdateEmployeeAsync(Employee employee)
    {
        var existingEmployee = await _employeeRepository.GetByIdAsync(employee.Id);
        if (existingEmployee == null)
        {
            throw new Exception("Employee not found.");
        }

        // Check for duplicate email (excluding current employee)
        var existingByEmail = await _employeeRepository.GetByEmailAsync(employee.Email);
        if (existingByEmail != null && existingByEmail.Id != employee.Id)
        {
            throw new Exception("An employee with this email already exists.");
        }

        // Update properties
        existingEmployee.FirstName = employee.FirstName;
        existingEmployee.LastName = employee.LastName;
        existingEmployee.Email = employee.Email;
        existingEmployee.Phone = employee.Phone;
        existingEmployee.DateOfBirth = employee.DateOfBirth;
        existingEmployee.Gender = employee.Gender;
        existingEmployee.MaritalStatus = employee.MaritalStatus;
        existingEmployee.NationalId = employee.NationalId;
        existingEmployee.TinNo = employee.TinNo;
        existingEmployee.BloodGroup = employee.BloodGroup;
        existingEmployee.PresentAddress = employee.PresentAddress;
        existingEmployee.PermanentAddress = employee.PermanentAddress;
        existingEmployee.ProfilePhoto = employee.ProfilePhoto;
        existingEmployee.DepartmentId = employee.DepartmentId;
        existingEmployee.DesignationId = employee.DesignationId;
        existingEmployee.ShiftId = employee.ShiftId;
        existingEmployee.SupervisorId = employee.SupervisorId;
        existingEmployee.DateOfJoining = employee.DateOfJoining;
        existingEmployee.DateOfConfirmation = employee.DateOfConfirmation;
        existingEmployee.EmploymentType = employee.EmploymentType;
        existingEmployee.Status = employee.Status;
        existingEmployee.IsTaxExempted = employee.IsTaxExempted;
        existingEmployee.BankAccountNo = employee.BankAccountNo;
        existingEmployee.BankName = employee.BankName;
        existingEmployee.BranchName = employee.BranchName;

        return await _employeeRepository.UpdateAsync(existingEmployee);
    }

    public async Task<bool> DeleteEmployeeAsync(long id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new Exception("Employee not found.");
        }

        // Soft delete - just mark as inactive
        employee.Status = EmployeeStatus.Terminated;
        await _employeeRepository.UpdateAsync(employee);
        return true;
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _employeeRepository.GetTotalCountAsync();
    }

    public async Task<int> GetActiveCountAsync()
    {
        return await _employeeRepository.GetActiveCountAsync();
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(long departmentId)
    {
        return await _employeeRepository.GetByDepartmentIdAsync(departmentId);
    }

    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
    {
        return await _employeeRepository.GetActiveEmployeesAsync();
    }

    /// <summary>
    /// Get all employees with navigation properties (Department, Designation, Shift, Manager, Supervisor)
    /// </summary>
    public async Task<IEnumerable<Employee>> GetAllEmployeesWithIncludesAsync()
    {
        return await _employeeRepository.GetAllWithIncludesAsync();
    }

    /// <summary>
    /// Get employee by ID with navigation properties populated
    /// </summary>
    public async Task<Employee?> GetEmployeeByIdWithIncludesAsync(long id)
    {
        return await _employeeRepository.GetByIdWithIncludesAsync(id);
    }

    /// <summary>
    /// Get active employees with navigation properties populated
    /// </summary>
    public async Task<IEnumerable<Employee>> GetActiveEmployeesWithIncludesAsync()
    {
        return await _employeeRepository.GetActiveEmployeesWithIncludesAsync();
    }

    private async Task<string> GenerateEmployeeCodeAsync()
    {
        // Generate employee code in format EMP001, EMP002, etc.
        var allEmployees = await _employeeRepository.GetAllAsync();
        var maxCode = allEmployees
            .Where(e => e.EmployeeCode.StartsWith("EMP"))
            .Select(e => e.EmployeeCode)
            .Select(e => int.TryParse(e.Substring(3), out var n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max();

        return $"EMP{(maxCode + 1):D3}";
    }
}
