using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Entities.Payroll;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRPayroll.Infrastructure.Repositories;

/// <summary>
/// Attendance repository implementation
/// </summary>
public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Attendance?> GetByEmployeeAndDateAsync(long employeeId, DateTime date)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == date);
    }

    public async Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _dbSet.Where(a => a.EmployeeId == employeeId).OrderByDescending(a => a.Date).ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetByDateRangeAsync(long employeeId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet.Where(a => a.EmployeeId == employeeId && a.Date >= startDate && a.Date <= endDate)
            .OrderByDescending(a => a.Date).ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date)
    {
        return await _dbSet.Where(a => a.Date == date).ToListAsync();
    }

    public async Task<int> GetTodayPresentCountAsync()
    {
        var today = DateTime.Today;
        return await _dbSet.CountAsync(a => a.Date == today && (a.Status == AttendanceStatus.Present || a.Status == AttendanceStatus.Late));
    }

    public async Task<int> GetTodayAbsentCountAsync()
    {
        var today = DateTime.Today;
        return await _dbSet.CountAsync(a => a.Date == today && a.Status == AttendanceStatus.Absent);
    }

    public async Task<int> GetTodayLateCountAsync()
    {
        var today = DateTime.Today;
        return await _dbSet.CountAsync(a => a.Date == today && a.Status == AttendanceStatus.Late);
    }

    public async Task<IEnumerable<Attendance>> GetAllWithIncludeAsync()
    {
        return await _dbSet.Include(a => a.Employee).OrderByDescending(a => a.Date).ToListAsync();
    }
}

/// <summary>
/// Leave repository implementation
/// </summary>
public class LeaveRepository : Repository<Leave>, ILeaveRepository
{
    public LeaveRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Leave?> GetByEmployeeAndDateRangeAsync(long employeeId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet.FirstOrDefaultAsync(l =>
            l.EmployeeId == employeeId &&
            ((l.StartDate >= startDate && l.StartDate <= endDate) ||
             (l.EndDate >= startDate && l.EndDate <= endDate)));
    }

    public async Task<IEnumerable<Leave>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _dbSet.Where(l => l.EmployeeId == employeeId).OrderByDescending(l => l.AppliedOn).ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetByStatusAsync(LeaveStatus status)
    {
        return await _dbSet.Where(l => l.Status == status).OrderByDescending(l => l.AppliedOn).ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetPendingApprovalsAsync(long approverId)
    {
        return await _dbSet.Where(l => l.Status == LeaveStatus.Pending).ToListAsync();
    }

    public async Task<int> GetPendingCountAsync()
    {
        return await _dbSet.CountAsync(l => l.Status == LeaveStatus.Pending);
    }
}

/// <summary>
/// LeaveType repository implementation
/// </summary>
public class LeaveTypeRepository : Repository<HRPayroll.Domain.Entities.HR.LeaveType>, ILeaveTypeRepository
{
    public LeaveTypeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<HRPayroll.Domain.Entities.HR.LeaveType?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(lt => lt.Code == code);
    }

    public async Task<IEnumerable<HRPayroll.Domain.Entities.HR.LeaveType>> GetActiveLeaveTypesAsync()
    {
        return await _dbSet.Where(lt => lt.IsActive).ToListAsync();
    }
}

/// <summary>
/// LeaveBalance repository implementation
/// </summary>
public class LeaveBalanceRepository : Repository<LeaveBalance>, ILeaveBalanceRepository
{
    public LeaveBalanceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<LeaveBalance?> GetByEmployeeAndLeaveTypeAndYearAsync(long employeeId, long leaveTypeId, int year)
    {
        return await _dbSet.FirstOrDefaultAsync(lb =>
            lb.EmployeeId == employeeId && lb.LeaveTypeId == leaveTypeId && lb.Year == year);
    }

    public async Task<IEnumerable<LeaveBalance>> GetByEmployeeIdAndYearAsync(long employeeId, int year)
    {
        return await _dbSet.Where(lb => lb.EmployeeId == employeeId && lb.Year == year).ToListAsync();
    }
}

/// <summary>
/// Holiday repository implementation
/// </summary>
public class HolidayRepository : Repository<Holiday>, IHolidayRepository
{
    public HolidayRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Holiday>> GetByYearAsync(int year)
    {
        return await _dbSet.Where(h => h.Date.Year == year).OrderBy(h => h.Date).ToListAsync();
    }

    public async Task<IEnumerable<Holiday>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet.Where(h => h.Date >= startDate && h.Date <= endDate).OrderBy(h => h.Date).ToListAsync();
    }

    public async Task<IEnumerable<Holiday>> GetNationalHolidaysAsync()
    {
        return await _dbSet.Where(h => h.Type == HolidayType.National).OrderBy(h => h.Date).ToListAsync();
    }
}
