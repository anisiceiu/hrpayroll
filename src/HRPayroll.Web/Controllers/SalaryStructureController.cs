using HRPayroll.Domain.Entities.Payroll;
using HRPayroll.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRPayroll.Web.Controllers;

[Authorize(Roles = "SystemAdmin")]
public class SalaryStructureController : Controller
{
    private readonly ISalaryStructureService _salaryStructureService;
    private readonly IEmployeeRepository _employeeRepository;

    public SalaryStructureController(
        ISalaryStructureService salaryStructureService,
        IEmployeeRepository employeeRepository)
    {
        _salaryStructureService = salaryStructureService;
        _employeeRepository = employeeRepository;
    }

    // GET: SalaryStructure
    public async Task<IActionResult> Index()
    {
        // Use GetAllSalaryStructuresWithIncludesAsync to populate navigation properties
        var salaryStructures = await _salaryStructureService.GetAllSalaryStructuresWithIncludesAsync();
        return View(salaryStructures);
    }

    // GET: SalaryStructure/Details/5
    public async Task<IActionResult> Details(long id)
    {
        // Use GetSalaryStructureByIdWithIncludesAsync to populate navigation properties
        var salaryStructure = await _salaryStructureService.GetSalaryStructureByIdWithIncludesAsync(id);
        if (salaryStructure == null)
        {
            return NotFound();
        }
        return View(salaryStructure);
    }

    // GET: SalaryStructure/Create
    public async Task<IActionResult> Create()
    {
        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.EmployeeList = new SelectList(employees, "Id", "FullName");
        return View();
    }

    // POST: SalaryStructure/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SalaryStructure salaryStructure)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _salaryStructureService.CreateSalaryStructureAsync(salaryStructure);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.EmployeeList = new SelectList(employees, "Id", "FullName", salaryStructure.EmployeeId);
        return View(salaryStructure);
    }

    // GET: SalaryStructure/Edit/5
    public async Task<IActionResult> Edit(long id)
    {
        // Use GetSalaryStructureByIdWithIncludesAsync to populate navigation properties
        var salaryStructure = await _salaryStructureService.GetSalaryStructureByIdWithIncludesAsync(id);
        if (salaryStructure == null)
        {
            return NotFound();
        }
        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.EmployeeList = new SelectList(employees, "Id", "FullName", salaryStructure.EmployeeId);
        return View(salaryStructure);
    }

    // POST: SalaryStructure/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, SalaryStructure salaryStructure)
    {
        if (id != salaryStructure.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _salaryStructureService.UpdateSalaryStructureAsync(salaryStructure);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.EmployeeList = new SelectList(employees, "Id", "FullName", salaryStructure.EmployeeId);
        return View(salaryStructure);
    }

    // GET: SalaryStructure/Delete/5
    public async Task<IActionResult> Delete(long id)
    {
        var salaryStructure = await _salaryStructureService.GetSalaryStructureByIdAsync(id);
        if (salaryStructure == null)
        {
            return NotFound();
        }
        return View(salaryStructure);
    }

    // POST: SalaryStructure/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        try
        {
            await _salaryStructureService.DeleteSalaryStructureAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return RedirectToAction(nameof(Delete), new { id });
        }
    }

    // GET: SalaryStructure/ByEmployee/5
    public async Task<IActionResult> ByEmployee(long employeeId)
    {
        // Use GetByEmployeeIdWithIncludesAsync to populate navigation properties
        var salaryStructure = await _salaryStructureService.GetByEmployeeIdWithIncludesAsync(employeeId);
        if (salaryStructure == null)
        {
            return NotFound();
        }
        return View("Details", salaryStructure);
    }
}
