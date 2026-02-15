using HRPayroll.Application.Services;
using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HRPayroll.Domain.Identity;

namespace HRPayroll.Web.Controllers;

[Authorize(Roles ="Employee")]
public class EmployeeDashboardController : Controller
{
    private readonly IAttendanceService _attendanceService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<EmployeeDashboardController> _logger;

    public EmployeeDashboardController(
        IAttendanceService attendanceService,
        IEmployeeRepository employeeRepository,
        UserManager<ApplicationUser> userManager,
        ILogger<EmployeeDashboardController> logger)
    {
        _attendanceService = attendanceService;
        _employeeRepository = employeeRepository;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Get the current logged-in employee's ID
    /// </summary>
    private async Task<long> GetCurrentEmployeeIdAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        if (!user.EmployeeId.HasValue)
        {
            throw new Exception("Employee profile not linked to this account");
        }
        
        return user.EmployeeId.Value;
    }

    /// <summary>
    /// Employee Dashboard - shows clock in/out buttons and today's status
    /// </summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            var employeeId = await GetCurrentEmployeeIdAsync();
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            var todayAttendance = await _attendanceService.GetTodayAttendanceAsync(employeeId);

            var model = new EmployeeDashboardViewModel
            {
                EmployeeId = employeeId,
                EmployeeName = employee?.FullName ?? "Unknown",
                Department = employee?.Department?.Name ?? "",
                Designation = employee?.Designation?.Name ?? "",
                ShiftStart = employee?.Shift?.StartTime.ToString(@"hh\:mm") ?? "",
                ShiftEnd = employee?.Shift?.EndTime.ToString(@"hh\:mm") ?? "",
                TodayAttendance = todayAttendance,
                IsClockedIn = todayAttendance?.ClockInTime != null,
                IsClockedOut = todayAttendance?.ClockOutTime != null,
                CurrentTime = DateTime.Now
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading employee dashboard");
            TempData["Error"] = ex.Message;
            return View(new EmployeeDashboardViewModel());
        }
    }

    /// <summary>
    /// Clock In action - records employee's clock in time
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClockIn()
    {
        try
        {
            var employeeId = await GetCurrentEmployeeIdAsync();
            var attendance = await _attendanceService.ClockInAsync(employeeId);
            
            TempData["Success"] = $"Clocked in at {attendance.ClockInTime:hh\\:mm\\:ss}";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clocking in");
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Clock Out action - records employee's clock out time
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClockOut()
    {
        try
        {
            var employeeId = await GetCurrentEmployeeIdAsync();
            var attendance = await _attendanceService.ClockOutAsync(employeeId);
            
            TempData["Success"] = $"Clocked out at {attendance.ClockOutTime:hh\\:mm\\:ss}. Total working hours: {attendance.WorkingHours:F2}";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clocking out");
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Get attendance history for the logged-in employee
    /// </summary>
    public async Task<IActionResult> MyAttendance(int? month, int? year)
    {
        try
        {
            var employeeId = await GetCurrentEmployeeIdAsync();
            var currentMonth = month ?? DateTime.Now.Month;
            var currentYear = year ?? DateTime.Now.Year;

            var startDate = new DateTime(currentYear, currentMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var attendances = await _attendanceService.GetByDateRangeAsync(employeeId, startDate, endDate);

            ViewBag.Month = currentMonth;
            ViewBag.Year = currentYear;

            return View(attendances.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading attendance history");
            TempData["Error"] = ex.Message;
            return View(new List<Attendance>());
        }
    }
}

/// <summary>
/// ViewModel for Employee Dashboard
/// </summary>
public class EmployeeDashboardViewModel
{
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; } = "";
    public string Department { get; set; } = "";
    public string Designation { get; set; } = "";
    public string ShiftStart { get; set; } = "";
    public string ShiftEnd { get; set; } = "";
    public Attendance? TodayAttendance { get; set; }
    public bool IsClockedIn { get; set; }
    public bool IsClockedOut { get; set; }
    public DateTime CurrentTime { get; set; }

    public string StatusText
    {
        get
        {
            if (IsClockedOut) return "Completed";
            if (IsClockedIn) return "Working";
            return "Not Started";
        }
    }

    public string? WorkingHoursText => TodayAttendance?.WorkingHours?.ToString("F2");
    public string? ClockInText => TodayAttendance?.ClockInTime?.ToString(@"hh\:mm\:ss");
    public string? ClockOutText => TodayAttendance?.ClockOutTime?.ToString(@"hh\:mm\:ss");
}
