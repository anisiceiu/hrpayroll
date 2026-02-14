using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Domain.Entities.HR;

namespace HRPayroll.Web.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly IDesignationService _designationService;
    private readonly IShiftService _shiftService;

    public EmployeeController(
        IEmployeeService employeeService,
        IDepartmentService departmentService,
        IDesignationService designationService,
        IShiftService shiftService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _designationService = designationService;
        _shiftService = shiftService;
    }

    private async Task PopulateDropdowns()
    {
        var departments = await _departmentService.GetActiveDepartmentsAsync();
        ViewBag.Departments = new SelectList(departments, "Id", "Name");
        
        var designations = await _designationService.GetActiveDesignationsAsync();
        ViewBag.Designations = new SelectList(designations, "Id", "Name");
        
        var shifts = await _shiftService.GetActiveShiftsAsync();
        ViewBag.Shifts = new SelectList(shifts, "Id", "Name");
    }

    private async Task<string> GenerateEmployeeCode()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        var lastEmployee = employees.OrderByDescending(e => e.Id).FirstOrDefault();
        
        if (lastEmployee == null)
        {
            return "EMP001";
        }
        
        // Extract number from existing code
        string existingCode = lastEmployee.EmployeeCode;
        if (existingCode.StartsWith("EMP") && int.TryParse(existingCode.Substring(3), out int lastNumber))
        {
            int newNumber = lastNumber + 1;
            return $"EMP{newNumber:D3}";
        }
        
        return $"EMP{(employees.Count() + 1):D3}";
    }

    public async Task<IActionResult> Index()
    {
        // Use GetAllEmployeesWithIncludesAsync to populate navigation properties
        var employees = await _employeeService.GetAllEmployeesWithIncludesAsync();
        return View(employees);
    }

    public async Task<IActionResult> Details(long id)
    {
        // Use GetEmployeeByIdWithIncludesAsync to populate navigation properties
        var employee = await _employeeService.GetEmployeeByIdWithIncludesAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDropdowns();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee employee)
    {
        try
        {
            if (ModelState.IsValid)
            {
                // Auto-generate employee code if not provided
                if (string.IsNullOrEmpty(employee.EmployeeCode))
                {
                    employee.EmployeeCode = await GenerateEmployeeCode();
                }
                
                await _employeeService.CreateEmployeeAsync(employee);
                TempData["Success"] = $"Employee created successfully! Employee Code: {employee.EmployeeCode}";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        await PopulateDropdowns();
        return View(employee);
    }

    public async Task<IActionResult> Edit(long id)
    {
        // Use GetEmployeeByIdWithIncludesAsync to populate navigation properties
        var employee = await _employeeService.GetEmployeeByIdWithIncludesAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        await PopulateDropdowns();
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, Employee employee)
    {
        try
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _employeeService.UpdateEmployeeAsync(employee);
                TempData["Success"] = "Employee updated successfully!";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        await PopulateDropdowns();
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _employeeService.DeleteEmployeeAsync(id);
            TempData["Success"] = "Employee deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }
}
