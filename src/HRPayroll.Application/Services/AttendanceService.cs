using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Interfaces;

namespace HRPayroll.Application.Services;

/// <summary>
/// Attendance service implementation
/// </summary>
public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public AttendanceService(IAttendanceRepository attendanceRepository, IEmployeeRepository employeeRepository)
    {
        _attendanceRepository = attendanceRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<Attendance>> GetAllAttendancesAsync()
    {
        return await _attendanceRepository.GetAllWithIncludeAsync();
    }

    public async Task<Attendance?> GetAttendanceByIdAsync(long id)
    {
        return await _attendanceRepository.GetByIdAsync(id);
    }

    public async Task<Attendance?> GetByEmployeeAndDateAsync(long employeeId, DateTime date)
    {
        return await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, date);
    }

    public async Task<Attendance> RecordAttendanceAsync(Attendance attendance)
    {
        // Check if attendance already exists for this employee and date
        var existing = await _attendanceRepository.GetByEmployeeAndDateAsync(attendance.EmployeeId, attendance.Date);
        if (existing != null)
        {
            throw new Exception("Attendance already recorded for this date.");
        }

        // Set status based on clock in time
        var employee = await _employeeRepository.GetByIdAsync(attendance.EmployeeId);
        if (employee?.Shift != null && attendance.ClockInTime.HasValue)
        {
            var graceTimeInMinutes = employee.Shift.GraceTimeMinutes;
            var allowedTime = employee.Shift.StartTime.TotalMinutes + graceTimeInMinutes;
            var clockInMinutes = attendance.ClockInTime.Value.TotalMinutes;
            
            if (clockInMinutes > allowedTime)
            {
                attendance.Status = AttendanceStatus.Late;
                attendance.LateMinutes = (int)(clockInMinutes - employee.Shift.StartTime.TotalMinutes);
            }
            else
            {
                attendance.Status = AttendanceStatus.Present;
            }
        }

        return await _attendanceRepository.AddAsync(attendance);
    }

    public async Task<Attendance> UpdateAttendanceAsync(Attendance attendance)
    {
        var existing = await _attendanceRepository.GetByIdAsync(attendance.Id);
        if (existing == null)
        {
            throw new Exception("Attendance record not found.");
        }

        existing.ClockInTime = attendance.ClockInTime;
        existing.ClockOutTime = attendance.ClockOutTime;
        existing.Remarks = attendance.Remarks;
        existing.Status = attendance.Status;

        return await _attendanceRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAttendanceAsync(long id)
    {
        var attendance = await _attendanceRepository.GetByIdAsync(id);
        if (attendance == null)
        {
            throw new Exception("Attendance record not found.");
        }

        await _attendanceRepository.DeleteAsync(attendance);
        return true;
    }

    public async Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _attendanceRepository.GetByEmployeeIdAsync(employeeId);
    }

    public async Task<IEnumerable<Attendance>> GetByDateRangeAsync(long employeeId, DateTime startDate, DateTime endDate)
    {
        return await _attendanceRepository.GetByDateRangeAsync(employeeId, startDate, endDate);
    }

    public async Task<int> GetTodayPresentCountAsync()
    {
        return await _attendanceRepository.GetTodayPresentCountAsync();
    }

    public async Task<int> GetTodayAbsentCountAsync()
    {
        return await _attendanceRepository.GetTodayAbsentCountAsync();
    }

    public async Task<int> GetTodayLateCountAsync()
    {
        return await _attendanceRepository.GetTodayLateCountAsync();
    }

    public async Task<bool> ImportFromBiometricAsync(IEnumerable<Attendance> attendances)
    {
        foreach (var attendance in attendances)
        {
            await _attendanceRepository.AddAsync(attendance);
        }

        return true;
    }

    /// <summary>
    /// Employee clocks in - creates a new attendance record for today
    /// </summary>
    public async Task<Attendance> ClockInAsync(long employeeId)
    {
        var today = DateTime.Today;
        
        // Check if already clocked in today
        var existing = await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, today);
        if (existing != null && existing.ClockInTime.HasValue)
        {
            throw new Exception("You have already clocked in today.");
        }

        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
        {
            throw new Exception("Employee not found.");
        }

        var now = DateTime.Now;
        var clockInTime = now.TimeOfDay;

        // Determine status based on shift timing
        var status = AttendanceStatus.Present;
        int? lateMinutes = null;

        if (employee.Shift != null)
        {
            var graceTimeInMinutes = employee.Shift.GraceTimeMinutes;
            var allowedTime = employee.Shift.StartTime.TotalMinutes + graceTimeInMinutes;
            var clockInMinutes = clockInTime.TotalMinutes;

            if (clockInMinutes > allowedTime)
            {
                status = AttendanceStatus.Late;
                lateMinutes = (int)(clockInMinutes - employee.Shift.StartTime.TotalMinutes);
            }
        }

        // If record exists but clock in was not set, update it
        if (existing != null)
        {
            existing.ClockInTime = clockInTime;
            existing.Status = status;
            existing.LateMinutes = lateMinutes;
            existing.EntryType = EntryType.Web;
            return await _attendanceRepository.UpdateAsync(existing);
        }

        // Create new attendance record
        var attendance = new Attendance
        {
            EmployeeId = employeeId,
            Date = today,
            ClockInTime = clockInTime,
            Status = status,
            LateMinutes = lateMinutes,
            EntryType = EntryType.Web
        };

        return await _attendanceRepository.AddAsync(attendance);
    }

    /// <summary>
    /// Employee clocks out - updates the existing attendance record for today
    /// </summary>
    public async Task<Attendance?> ClockOutAsync(long employeeId)
    {
        var today = DateTime.Today;
        var existing = await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, today);

        if (existing == null || !existing.ClockInTime.HasValue)
        {
            throw new Exception("You have not clocked in today.");
        }

        if (existing.ClockOutTime.HasValue)
        {
            throw new Exception("You have already clocked out today.");
        }

        var now = DateTime.Now;
        var clockOutTime = now.TimeOfDay;
        var clockInTime = existing.ClockInTime.Value;

        // Calculate working hours
        double totalMinutes = (clockOutTime - clockInTime).TotalMinutes;
        if (totalMinutes < 0)
        {
            // Handle overnight shifts
            totalMinutes += 24 * 60;
        }
        var workingHours = (decimal)(totalMinutes / 60);

        // Check for early leaving
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        int? earlyLeavingMinutes = null;
        if (employee?.Shift != null)
        {
            var expectedEndMinutes = employee.Shift.EndTime.TotalMinutes;
            var actualEndMinutes = clockOutTime.TotalMinutes;

            if (actualEndMinutes < expectedEndMinutes - 30) // 30 min grace
            {
                earlyLeavingMinutes = (int)(expectedEndMinutes - actualEndMinutes);
            }

            // Calculate overtime if worked more than scheduled
            if (totalMinutes > (employee.Shift.EndTime - employee.Shift.StartTime).TotalMinutes)
            {
                var overtimeMinutes = totalMinutes - (employee.Shift.EndTime - employee.Shift.StartTime).TotalMinutes;
                existing.OvertimeHours = (decimal)(overtimeMinutes / 60);
            }
        }

        existing.ClockOutTime = clockOutTime;
        existing.WorkingHours = workingHours;
        existing.EarlyLeavingMinutes = earlyLeavingMinutes;
        existing.EntryType = EntryType.Web;

        return await _attendanceRepository.UpdateAsync(existing);
    }

    /// <summary>
    /// Get today's attendance record for an employee
    /// </summary>
    public async Task<Attendance?> GetTodayAttendanceAsync(long employeeId)
    {
        return await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, DateTime.Today);
    }
}

/// <summary>
/// LeaveType service implementation
/// </summary>
public class LeaveTypeService : ILeaveTypeService
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;

    public LeaveTypeService(ILeaveTypeRepository leaveTypeRepository)
    {
        _leaveTypeRepository = leaveTypeRepository;
    }

    public async Task<IEnumerable<HRPayroll.Domain.Entities.HR.LeaveType>> GetAllLeaveTypesAsync()
    {
        return await _leaveTypeRepository.GetAllAsync();
    }

    public async Task<HRPayroll.Domain.Entities.HR.LeaveType?> GetLeaveTypeByIdAsync(long id)
    {
        return await _leaveTypeRepository.GetByIdAsync(id);
    }

    public async Task<HRPayroll.Domain.Entities.HR.LeaveType> CreateLeaveTypeAsync(HRPayroll.Domain.Entities.HR.LeaveType leaveType)
    {
        var existing = await _leaveTypeRepository.GetByCodeAsync(leaveType.Code);
        if (existing != null)
        {
            throw new Exception("A leave type with this code already exists.");
        }

        leaveType.IsActive = true;
        return await _leaveTypeRepository.AddAsync(leaveType);
    }

    public async Task<HRPayroll.Domain.Entities.HR.LeaveType> UpdateLeaveTypeAsync(HRPayroll.Domain.Entities.HR.LeaveType leaveType)
    {
        var existing = await _leaveTypeRepository.GetByIdAsync(leaveType.Id);
        if (existing == null)
        {
            throw new Exception("Leave type not found.");
        }

        existing.Name = leaveType.Name;
        existing.NameBN = leaveType.NameBN;
        existing.Description = leaveType.Description;
        existing.IsPaidLeave = leaveType.IsPaidLeave;
        existing.IsCarryForwardAllowed = leaveType.IsCarryForwardAllowed;
        existing.MaxCarryForwardDays = leaveType.MaxCarryForwardDays;
        existing.MaxAccumulationDays = leaveType.MaxAccumulationDays;
        existing.RequiresApproval = leaveType.RequiresApproval;
        existing.RequiresDocument = leaveType.RequiresDocument;
        existing.ColorCode = leaveType.ColorCode;
        existing.IsActive = leaveType.IsActive;

        return await _leaveTypeRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteLeaveTypeAsync(long id)
    {
        var leaveType = await _leaveTypeRepository.GetByIdAsync(id);
        if (leaveType == null)
        {
            throw new Exception("Leave type not found.");
        }

        leaveType.IsActive = false;
        await _leaveTypeRepository.UpdateAsync(leaveType);
        return true;
    }

    public async Task<IEnumerable<HRPayroll.Domain.Entities.HR.LeaveType>> GetActiveLeaveTypesAsync()
    {
        return await _leaveTypeRepository.GetActiveLeaveTypesAsync();
    }
}

/// <summary>
/// Holiday service implementation
/// </summary>
public class HolidayService : IHolidayService
{
    private readonly IHolidayRepository _holidayRepository;

    public HolidayService(IHolidayRepository holidayRepository)
    {
        _holidayRepository = holidayRepository;
    }

    public async Task<IEnumerable<Holiday>> GetAllHolidaysAsync()
    {
        return await _holidayRepository.GetAllAsync();
    }

    public async Task<Holiday?> GetHolidayByIdAsync(long id)
    {
        return await _holidayRepository.GetByIdAsync(id);
    }

    public async Task<Holiday> CreateHolidayAsync(Holiday holiday)
    {
        // Check for duplicate date
        var existing = await _holidayRepository.GetByDateRangeAsync(holiday.Date, holiday.Date);
        if (existing.Any())
        {
            throw new Exception("A holiday already exists for this date.");
        }

        return await _holidayRepository.AddAsync(holiday);
    }

    public async Task<Holiday> UpdateHolidayAsync(Holiday holiday)
    {
        var existing = await _holidayRepository.GetByIdAsync(holiday.Id);
        if (existing == null)
        {
            throw new Exception("Holiday not found.");
        }

        existing.Name = holiday.Name;
        existing.NameBN = holiday.NameBN;
        existing.Date = holiday.Date;
        existing.Type = holiday.Type;
        existing.Description = holiday.Description;
        existing.IsRepeatAnnually = holiday.IsRepeatAnnually;

        return await _holidayRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteHolidayAsync(long id)
    {
        var holiday = await _holidayRepository.GetByIdAsync(id);
        if (holiday == null)
        {
            throw new Exception("Holiday not found.");
        }

        await _holidayRepository.DeleteAsync(holiday);
        return true;
    }

    public async Task<IEnumerable<Holiday>> GetByYearAsync(int year)
    {
        return await _holidayRepository.GetByYearAsync(year);
    }

    public async Task<IEnumerable<Holiday>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _holidayRepository.GetByDateRangeAsync(startDate, endDate);
    }
}
