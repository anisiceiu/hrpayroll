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
        return await _dbSet.Where(e => e.Status == EmployeeStatus.Active).ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<int> GetActiveCountAsync()
    {
        return await _dbSet.CountAsync(e => e.Status == EmployeeStatus.Active);
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

    public async Task<Shift?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.Code == code);
    }

    public async Task<IEnumerable<Shift>> GetActiveShiftsAsync()
    {
        return await _dbSet.Where(s => s.IsActive).ToListAsync();
    }
}
