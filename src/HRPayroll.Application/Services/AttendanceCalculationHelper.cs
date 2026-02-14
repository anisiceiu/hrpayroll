using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Interfaces;

namespace HRPayroll.Application.Services;

/// <summary>
/// DTO for holding attendance calculation results
/// </summary>
public class AttendanceCalculationResult
{
    public int TotalCalendarDays { get; set; }
    public int WeekendDays { get; set; }
    public int HolidayDays { get; set; }
    public int WorkingDays { get; set; }
    public int PresentDays { get; set; }
    public int LateDays { get; set; }
    public int HalfDays { get; set; }
    public int AbsentDays { get; set; }
    public decimal PaidLeaveDays { get; set; }
    public decimal UnpaidLeaveDays { get; set; }
    public int PaidDays { get; set; }
}

/// <summary>
/// Helper class for calculating working days and paid days from attendance and leave data
/// </summary>
public class AttendanceCalculationHelper
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly ILeaveRepository _leaveRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IHolidayRepository _holidayRepository;

    public AttendanceCalculationHelper(
        IAttendanceRepository attendanceRepository,
        ILeaveRepository leaveRepository,
        ILeaveTypeRepository leaveTypeRepository,
        IHolidayRepository holidayRepository)
    {
        _attendanceRepository = attendanceRepository;
        _leaveRepository = leaveRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _holidayRepository = holidayRepository;
    }

    /// <summary>
    /// Calculate working days and paid days for an employee within a date range
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="startDate">Start date of the period</param>
    /// <param name="endDate">End date of the period</param>
    /// <returns>AttendanceCalculationResult containing all calculated values</returns>
    public async Task<AttendanceCalculationResult> CalculateAsync(
        long employeeId, 
        DateTime startDate, 
        DateTime endDate)
    {
        var result = new AttendanceCalculationResult();

        // Step 1: Calculate total calendar days in the period
        result.TotalCalendarDays = (endDate - startDate).Days + 1;

        // Step 2: Calculate weekend days (Saturday and Sunday)
        result.WeekendDays = CountWeekendDays(startDate, endDate);

        // Step 3: Get holidays within the period
        var holidays = await _holidayRepository.GetByDateRangeAsync(startDate, endDate);
        var holidayList = holidays.ToList();
        result.HolidayDays = holidayList.Count;

        // Step 4: Calculate working days (excluding weekends and holidays)
        result.WorkingDays = result.TotalCalendarDays - result.WeekendDays - result.HolidayDays;

        // Step 5: Get attendance records for the period
        var attendanceRecords = (await _attendanceRepository.GetByDateRangeAsync(
            employeeId, startDate, endDate)).ToList();

        // Count attendance by status
        result.PresentDays = attendanceRecords.Count(a => a.Status == AttendanceStatus.Present);
        result.LateDays = attendanceRecords.Count(a => a.Status == AttendanceStatus.Late);
        result.HalfDays = attendanceRecords.Count(a => a.Status == AttendanceStatus.HalfDay);
        result.AbsentDays = attendanceRecords.Count(a => a.Status == AttendanceStatus.Absent);

        // Step 6: Get approved leaves within the period
        var allLeaves = await _leaveRepository.GetByEmployeeIdAsync(employeeId);
        var leavesInPeriod = allLeaves
            .Where(l => l.Status == LeaveStatus.Approved &&
                        l.StartDate <= endDate &&
                        l.EndDate >= startDate)
            .ToList();

        // Step 7: Calculate paid and unpaid leave days
        await CalculateLeaveDaysAsync(leavesInPeriod, startDate, endDate, result);

        // Step 8: Calculate final paid days
        // Paid Days = Present + Late + Half Days + Paid Leave + Holidays
        result.PaidDays = result.PresentDays + 
                          result.LateDays + 
                          result.HalfDays + 
                          (int)result.PaidLeaveDays + 
                          result.HolidayDays;

        // Ensure paid days doesn't exceed working days
        if (result.PaidDays > result.WorkingDays)
        {
            result.PaidDays = result.WorkingDays;
        }

        return result;
    }

    /// <summary>
    /// Calculate working days only (simpler version)
    /// </summary>
    public async Task<int> CalculateWorkingDaysAsync(DateTime startDate, DateTime endDate)
    {
        var totalDays = (endDate - startDate).Days + 1;
        var weekendDays = CountWeekendDays(startDate, endDate);
        
        var holidays = await _holidayRepository.GetByDateRangeAsync(startDate, endDate);
        var holidayDays = holidays.Count();

        return totalDays - weekendDays - holidayDays;
    }

    /// <summary>
    /// Calculate paid days only (simpler version)
    /// </summary>
    public async Task<int> CalculatePaidDaysAsync(
        long employeeId, 
        DateTime startDate, 
        DateTime endDate)
    {
        var result = await CalculateAsync(employeeId, startDate, endDate);
        return result.PaidDays;
    }

    /// <summary>
    /// Get today's attendance for a specific employee
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <returns>Today's attendance record if exists, null otherwise</returns>
    public async Task<Attendance?> GetTodayAttendanceAsync(long employeeId)
    {
        return await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, DateTime.Today);
    }

    /// <summary>
    /// Get all employees' attendance for today
    /// </summary>
    /// <returns>List of all employees' attendance for today</returns>
    public async Task<IEnumerable<Attendance>> GetAllTodayAttendanceAsync()
    {
        return await _attendanceRepository.GetByDateAsync(DateTime.Today);
    }

    /// <summary>
    /// Calculate today's attendance summary for an employee
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <returns>Attendance result for today</returns>
    public async Task<AttendanceCalculationResult> CalculateTodayAttendanceAsync(long employeeId)
    {
        var today = DateTime.Today;
        return await CalculateAsync(employeeId, today, today);
    }

    /// <summary>
    /// Get attendance statistics for today (all employees)
    /// </summary>
    public async Task<(int Present, int Absent, int Late, int HalfDay, int OnLeave)> GetTodayStatisticsAsync()
    {
        var todayAttendance = await _attendanceRepository.GetByDateAsync(DateTime.Today);
        var attendanceList = todayAttendance.ToList();

        var present = attendanceList.Count(a => a.Status == AttendanceStatus.Present);
        var absent = attendanceList.Count(a => a.Status == AttendanceStatus.Absent);
        var late = attendanceList.Count(a => a.Status == AttendanceStatus.Late);
        var halfDay = attendanceList.Count(a => a.Status == AttendanceStatus.HalfDay);
        var onLeave = attendanceList.Count(a => a.Status == AttendanceStatus.Leave);

        return (present, absent, late, halfDay, onLeave);
    }

    /// <summary>
    /// Count weekend days (Saturday and Sunday) in a date range
    /// </summary>
    private int CountWeekendDays(DateTime startDate, DateTime endDate)
    {
        var weekendDays = 0;
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || 
                date.DayOfWeek == DayOfWeek.Sunday)
            {
                weekendDays++;
            }
        }
        return weekendDays;
    }

    /// <summary>
    /// Calculate paid and unpaid leave days from approved leaves
    /// </summary>
    private async Task CalculateLeaveDaysAsync(
        List<Leave> leavesInPeriod,
        DateTime periodStart,
        DateTime periodEnd,
        AttendanceCalculationResult result)
    {
        foreach (var leave in leavesInPeriod)
        {
            // Calculate the overlap between leave period and payroll period
            var leaveStart = leave.StartDate < periodStart ? periodStart : leave.StartDate;
            var leaveEnd = leave.EndDate > periodEnd ? periodEnd : leave.EndDate;
            
            // Calculate overlap days
            var overlapDays = Convert.ToDecimal((leaveEnd - leaveStart).Days + 1);

            // Adjust for half-day leaves
            if (leave.IsHalfDay)
            {
                overlapDays = 0.5m;
            }

            // Get the leave type to check if it's paid
            var leaveType = await _leaveTypeRepository.GetByIdAsync(leave.LeaveTypeId);
            
            if (leaveType?.IsPaidLeave == true)
            {
                result.PaidLeaveDays += overlapDays;
            }
            else
            {
                result.UnpaidLeaveDays += overlapDays;
            }
        }
    }
}
