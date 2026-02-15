using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Domain.Entities.HR;

namespace HRPayroll.Web.Controllers;

[Authorize(Roles = "SystemAdmin")]
public class ShiftController : Controller
{
    private readonly IShiftService _shiftService;
    private readonly IEmployeeShiftService _employeeShiftService;
    private readonly IEmployeeRepository _employeeRepository;

    public ShiftController(
        IShiftService shiftService,
        IEmployeeShiftService employeeShiftService,
        IEmployeeRepository employeeRepository)
    {
        _shiftService = shiftService;
        _employeeShiftService = employeeShiftService;
        _employeeRepository = employeeRepository;
    }

    // ==================== SHIFT MANAGEMENT ====================

    public async Task<IActionResult> Index()
    {
        // Use GetAllShiftsWithIncludesAsync to populate navigation properties
        var shifts = await _shiftService.GetAllShiftsWithIncludesAsync();
        return View(shifts);
    }

    public async Task<IActionResult> Details(long id)
    {
        // Use GetShiftByIdWithIncludesAsync to populate navigation properties
        var shift = await _shiftService.GetShiftByIdWithIncludesAsync(id);
        if (shift == null)
        {
            return NotFound();
        }
        return View(shift);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Shift shift)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _shiftService.CreateShiftAsync(shift);
                TempData["Success"] = "Shift created successfully!";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return View(shift);
    }

    public async Task<IActionResult> Edit(long id)
    {
        // Use GetShiftByIdWithIncludesAsync to populate navigation properties
        var shift = await _shiftService.GetShiftByIdWithIncludesAsync(id);
        if (shift == null)
        {
            return NotFound();
        }
        return View(shift);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, Shift shift)
    {
        try
        {
            if (id != shift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _shiftService.UpdateShiftAsync(shift);
                TempData["Success"] = "Shift updated successfully!";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return View(shift);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _shiftService.DeleteShiftAsync(id);
            TempData["Success"] = "Shift deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    // ==================== EMPLOYEE SHIFT ASSIGNMENT ====================

    /// <summary>
    /// List all employee shift assignments
    /// </summary>
    public async Task<IActionResult> EmployeeShiftList()
    {
        var employeeShifts = await _employeeShiftService.GetAllEmployeeShiftsAsync();
        return View(employeeShifts);
    }

    /// <summary>
    /// Get current shift assignment for an employee
    /// </summary>
    public async Task<IActionResult> GetEmployeeShift(long employeeId)
    {
        var currentShift = await _employeeShiftService.GetCurrentAssignmentAsync(employeeId);
        if (currentShift == null)
        {
            return NotFound();
        }
        return View("EmployeeShiftDetails", currentShift);
    }

    /// <summary>
    /// Assign shift to employee - GET
    /// </summary>
    public async Task<IActionResult> AssignShift(long? employeeId = null)
    {
        var shifts = await _shiftService.GetActiveShiftsAsync();
        ViewBag.Shifts = new SelectList(shifts, "Id", "Name");

        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.Employees = new SelectList(employees, "Id", "FullName", employeeId);

        return View();
    }

    /// <summary>
    /// Assign shift to employee - POST
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignShift(EmployeeShift employeeShift)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _employeeShiftService.AssignShiftAsync(employeeShift);
                TempData["Success"] = "Shift assigned successfully!";
                return RedirectToAction(nameof(EmployeeShiftList));
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        // Repopulate dropdowns
        var shifts = await _shiftService.GetActiveShiftsAsync();
        ViewBag.Shifts = new SelectList(shifts, "Id", "Name", employeeShift.ShiftId);

        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.Employees = new SelectList(employees, "Id", "FullName", employeeShift.EmployeeId);

        return View(employeeShift);
    }

    /// <summary>
    /// Edit employee shift assignment - GET
    /// </summary>
    public async Task<IActionResult> EditEmployeeShift(long id)
    {
        var employeeShift = await _employeeShiftService.GetEmployeeShiftByIdAsync(id);
        if (employeeShift == null)
        {
            return NotFound();
        }

        var shifts = await _shiftService.GetActiveShiftsAsync();
        ViewBag.Shifts = new SelectList(shifts, "Id", "Name", employeeShift.ShiftId);

        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.Employees = new SelectList(employees, "Id", "FullName", employeeShift.EmployeeId);

        return View(employeeShift);
    }

    /// <summary>
    /// Edit employee shift assignment - POST
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEmployeeShift(long id, EmployeeShift employeeShift)
    {
        try
        {
            if (id != employeeShift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _employeeShiftService.UpdateEmployeeShiftAsync(employeeShift);
                TempData["Success"] = "Shift assignment updated successfully!";
                return RedirectToAction(nameof(EmployeeShiftList));
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        // Repopulate dropdowns
        var shifts = await _shiftService.GetActiveShiftsAsync();
        ViewBag.Shifts = new SelectList(shifts, "Id", "Name", employeeShift.ShiftId);

        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.Employees = new SelectList(employees, "Id", "FullName", employeeShift.EmployeeId);

        return View(employeeShift);
    }

    /// <summary>
    /// Remove employee shift assignment
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveEmployeeShift(long id)
    {
        try
        {
            await _employeeShiftService.RemoveAssignmentAsync(id);
            TempData["Success"] = "Shift assignment removed successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(EmployeeShiftList));
    }

    /// <summary>
    /// View shift details with assigned employees
    /// </summary>
    public async Task<IActionResult> ShiftEmployees(long id)
    {
        var shift = await _shiftService.GetShiftByIdWithIncludesAsync(id);
        if (shift == null)
        {
            return NotFound();
        }

        var employeeShifts = await _employeeShiftService.GetByShiftIdAsync(id);
        ViewBag.AssignedEmployees = employeeShifts;
        
        return View(shift);
    }
}
