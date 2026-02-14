using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRPayroll.Infrastructure.Repositories;

/// <summary>
/// Employee repository implementation
/// </summary>
public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get all employees with navigation properties populated
    /// </summary>
    public async Task<IEnumerable<Employee>> GetAllWithIncludesAsync()
    {
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .Include(e => e.Shift)
            .Include(e => e.Manager)
            .Include(e => e.Supervisor)
            .ToListAsync();
    }

    /// <summary>
    /// Get employee by ID with navigation properties populated
    /// </summary>
    public async Task<Employee?> GetByIdWithIncludesAsync(long id)
    {
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .Include(e => e.Shift)
            .Include(e => e.Manager)
            .Include(e => e.Supervisor)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Employee?> GetByEmployeeCodeAsync(string employeeCode)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
    }

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(long departmentId)
    {
        return await _dbSet.Where(e => e.DepartmentId == departmentId).ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByStatusAsync(EmployeeStatus status)
    {
        return await _dbSet.Where(e => e.Status == status).ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
    {
        return await _dbSet.Include(e=> e.SalaryStructure).Where(e => e.Status == EmployeeStatus.Active).ToListAsync();
    }

    /// <summary>
    /// Get active employees with navigation properties populated
    /// </summary>
    public async Task<IEnumerable<Employee>> GetActiveEmployeesWithIncludesAsync()
    {
        return await _dbSet
            .Where(e => e.Status == EmployeeStatus.Active)
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .Include(e => e.Shift)
            .Include(e => e.Manager)
            .Include(e => e.Supervisor)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<int> GetActiveCountAsync()
    {
        return await _dbSet.CountAsync(e => e.Status == EmployeeStatus.Active);
    }

    /// <summary>
    /// Get active employees with salary structure (for payroll)
    /// </summary>
    public async Task<IEnumerable<Employee>> GetActiveEmployeesWithSalaryStructureAsync()
    {
        return await _dbSet
            .Where(e => e.Status == EmployeeStatus.Active)
            .Include(e => e.SalaryStructure)
            .ToListAsync();
    }
}

/// <summary>
/// Department repository implementation
/// </summary>
public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get all departments with navigation properties populated
    /// </summary>
    public async Task<IEnumerable<Department>> GetAllWithIncludesAsync()
    {
        return await _dbSet
            .Include(d => d.ParentDepartment)
            .Include(d => d.HeadOfDepartment)
            .Include(d => d.Designations)
            .ToListAsync();
    }

    public async Task<Department?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(d => d.Code == code);
    }

    public async Task<IEnumerable<Department>> GetActiveDepartmentsAsync()
    {
        return await _dbSet.Where(d => d.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Department>> GetSubDepartmentsAsync(long parentId)
    {
        return await _dbSet.Where(d => d.ParentDepartmentId == parentId).ToListAsync();
    }
}

/// <summary>
/// Designation repository implementation
/// </summary>
public class DesignationRepository : Repository<Designation>, IDesignationRepository
{
    public DesignationRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get all designations with navigation properties populated
    /// </summary>
    public async Task<IEnumerable<Designation>> GetAllWithIncludesAsync()
    {
        return await _dbSet
            .Include(d => d.Department)
            .ToListAsync();
    }

    public async Task<Designation?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(d => d.Code == code);
    }

    public async Task<IEnumerable<Designation>> GetByDepartmentIdAsync(long departmentId)
    {
        return await _dbSet.Where(d => d.DepartmentId == departmentId).ToListAsync();
    }

    public async Task<IEnumerable<Designation>> GetActiveDesignationsAsync()
    {
        return await _dbSet.Where(d => d.IsActive).ToListAsync();
    }
}

/// <summary>
/// Shift repository implementation
/// </summary>
public class ShiftRepository : Repository<Shift>, IShiftRepository
{
    public ShiftRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get all shifts with navigation properties (Employees)
    /// </summary>
    public async Task<IEnumerable<Shift>> GetAllWithIncludesAsync()
    {
        return await _dbSet
            .Include(s => s.Employees)
                .ThenInclude(e => e.Department)
            .ToListAsync();
    }

    /// <summary>
    /// Get shift by ID with navigation properties
    /// </summary>
    public async Task<Shift?> GetByIdWithIncludesAsync(long id)
    {
        return await _dbSet
            .Include(s => s.Employees)
                .ThenInclude(e => e.Department)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Shift?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.Code == code);
    }

    public async Task<IEnumerable<Shift>> GetActiveShiftsAsync()
    {
        return await _dbSet.Where(s => s.IsActive).ToListAsync();
    }

    /// <summary>
    /// Get active shifts with navigation properties
    /// </summary>
    public async Task<IEnumerable<Shift>> GetActiveShiftsWithIncludesAsync()
    {
        return await _dbSet
            .Where(s => s.IsActive)
            .Include(s => s.Employees)
                .ThenInclude(e => e.Department)
            .ToListAsync();
    }
}
