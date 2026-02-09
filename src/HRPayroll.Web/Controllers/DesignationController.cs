using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Domain.Entities.HR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRPayroll.Web.Controllers;

public class DesignationController : Controller
{
    private readonly IDesignationService _designationService;
    private readonly IDepartmentService _departmentService;

    public DesignationController(
        IDesignationService designationService,
        IDepartmentService departmentService)
    {
        _designationService = designationService;
        _departmentService = departmentService;
    }

    private async Task PopulateDropdowns()
    {
        var departments = await _departmentService.GetActiveDepartmentsAsync();
        ViewBag.Departments = new SelectList(departments, "Id", "Name");
    }

    public async Task<IActionResult> Index()
    {
        var designations = await _designationService.GetAllDesignationsAsync();
        return View(designations);
    }

    public async Task<IActionResult> Details(long id)
    {
        var designation = await _designationService.GetDesignationByIdAsync(id);
        if (designation == null)
        {
            return NotFound();
        }
        return View(designation);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDropdowns();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Designation designation)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _designationService.CreateDesignationAsync(designation);
                TempData["Success"] = "Designation created successfully!";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        await PopulateDropdowns();
        return View(designation);
    }

    public async Task<IActionResult> Edit(long id)
    {
        var designation = await _designationService.GetDesignationByIdAsync(id);
        if (designation == null)
        {
            return NotFound();
        }

        await PopulateDropdowns();
        return View(designation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, Designation designation)
    {
        try
        {
            if (id != designation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _designationService.UpdateDesignationAsync(designation);
                TempData["Success"] = "Designation updated successfully!";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        await PopulateDropdowns();
        return View(designation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _designationService.DeleteDesignationAsync(id);
            TempData["Success"] = "Designation deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }
}
