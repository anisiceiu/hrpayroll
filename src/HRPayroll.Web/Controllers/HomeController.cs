using HRPayroll.Application.Services;
using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRPayroll.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDesignationRepository _designationRepository;
    private readonly IShiftRepository _shiftRepository;
    private readonly AttendanceCalculationHelper  _attendanceCalculationHelper;
    public HomeController(ILogger<HomeController> logger, IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository,
        IShiftRepository shiftRepository,IDesignationRepository  designationRepository, AttendanceCalculationHelper attendanceCalculationHelper)
    {
        _logger = logger;
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _shiftRepository = shiftRepository;
        _designationRepository = designationRepository;
        _attendanceCalculationHelper = attendanceCalculationHelper;
    }

    public async Task<IActionResult> Index()
    {
        DashboardViewModel dashboardViewModel = new DashboardViewModel
        {
            TotalEmployees = await _employeeRepository.GetActiveCountAsync(),
            TotalDepartments = await _departmentRepository.GetDepartmentCountAsync(),
            TotalDesignations = await _designationRepository.GetDesignationCountAsync(),
            TotalShifts = await _shiftRepository.GetShiftCountAsync()
        };
        
        var (present, absent, late, halfDay, onLeave) = await _attendanceCalculationHelper.GetTodayStatisticsAsync();

        dashboardViewModel.TodayPresent = present;
        dashboardViewModel.TodayAbsent = absent;
        dashboardViewModel.TodayLate = late;
        dashboardViewModel.TodayHalfDay = halfDay;
        dashboardViewModel.TodayLeave = onLeave;

        dashboardViewModel.Attendances = (await _attendanceCalculationHelper.GetAllTodayAttendanceAsync()).ToList();

        return View(dashboardViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
