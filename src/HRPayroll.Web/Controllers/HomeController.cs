using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRPayroll.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDesignationRepository _designationRepository;
    private readonly IShiftRepository _shiftRepository;
    public HomeController(ILogger<HomeController> logger, IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, IShiftRepository shiftRepository,IDesignationRepository  designationRepository)
    {
        _logger = logger;
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _shiftRepository = shiftRepository;
        _designationRepository = designationRepository;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalEmployees = await _employeeRepository.GetActiveCountAsync();
        ViewBag.TotalDepartments = await _departmentRepository.GetDepartmentCountAsync();
        ViewBag.TotalDesignations = await _designationRepository.GetDesignationCountAsync();
        ViewBag.TotalShifts = await _shiftRepository.GetShiftCountAsync();
      

        return View();
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
