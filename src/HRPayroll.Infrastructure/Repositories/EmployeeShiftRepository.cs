using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRPayroll.Infrastructure.Repositories;

/// <summary>
/// EmployeeShift repository implementation
/// </summary>
public class EmployeeShiftRepository : Repository<EmployeeShift>, IEmployeeShiftRepository
{
    public EmployeeShiftRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get all employee shifts with navigation properties
    /// </summary>
    public async Task<IEnumerable<EmployeeShift>> GetAllWithIncludesAsync()
    {
        return await _dbSet
            .Include(es => es.Employee)
                .ThenInclude(e => e!.Department)
            .Include(es => es.Shift)
            .ToListAsync();
    }

    /// <summary>
    /// Get employee shift by ID with navigation properties
    /// </summary>
    public async Task<EmployeeShift?> GetByIdWithIncludesAsync(long id)
    {
        return await _dbSet
            .Include(es => es.Employee)
                .ThenInclude(e => e!.Department)
            .Include(es => es.Shift)
            .FirstOrDefaultAsync(es => es.Id == id);
    }

    public async Task<IEnumerable<EmployeeShift>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _dbSet
            .Where(es => es.EmployeeId == employeeId)
            .OrderByDescending(es => es.EffectiveFrom)
            .ToListAsync();
    }

    public async Task<IEnumerable<EmployeeShift>> GetByShiftIdAsync(long shiftId)
    {
        return await _dbSet
            .Where(es => es.ShiftId == shiftId)
            .Include(es => es.Employee)
            .ToListAsync();
    }

    /// <summary>
    /// Get current active assignment for an employee
    /// </summary>
    public async Task<EmployeeShift?> GetCurrentAssignmentAsync(long employeeId)
    {
        var today = DateTime.Today;
        return await _dbSet
            .Include(es => es.Shift)
            .Where(es => es.EmployeeId == employeeId 
                && es.IsActive 
                && es.EffectiveFrom <= today 
                && (es.EffectiveTo == null || es.EffectiveTo >= today))
            .OrderByDescending(es => es.EffectiveFrom)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get current assignment with full navigation properties
    /// </summary>
    public async Task<EmployeeShift?> GetCurrentAssignmentWithIncludesAsync(long employeeId)
    {
        var today = DateTime.Today;
        return await _dbSet
            .Include(es => es.Employee)
                .ThenInclude(e => e!.Department)
            .Include(es => es.Shift)
            .Where(es => es.EmployeeId == employeeId 
                && es.IsActive 
                && es.EffectiveFrom <= today 
                && (es.EffectiveTo == null || es.EffectiveTo >= today))
            .OrderByDescending(es => es.EffectiveFrom)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<EmployeeShift>> GetActiveAssignmentsAsync()
    {
        var today = DateTime.Today;
        return await _dbSet
            .Where(es => es.IsActive 
                && es.EffectiveFrom <= today 
                && (es.EffectiveTo == null || es.EffectiveTo >= today))
            .Include(es => es.Employee)
            .Include(es => es.Shift)
            .ToListAsync();
    }

    public async Task<IEnumerable<EmployeeShift>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(es => es.EffectiveFrom <= endDate 
                && (es.EffectiveTo == null || es.EffectiveTo >= startDate))
            .Include(es => es.Employee)
            .Include(es => es.Shift)
            .ToListAsync();
    }

    /// <summary>
    /// Get assignments by employee ID with navigation properties
    /// </summary>
    public async Task<IEnumerable<EmployeeShift>> GetByEmployeeIdWithIncludesAsync(long employeeId)
    {
        return await _dbSet
            .Where(es => es.EmployeeId == employeeId)
            .Include(es => es.Employee)
                .ThenInclude(e => e!.Department)
            .Include(es => es.Shift)
            .OrderByDescending(es => es.EffectiveFrom)
            .ToListAsync();
    }
}
