using Microsoft.AspNetCore.Mvc;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Domain.Entities.HR;

namespace HRPayroll.Web.Controllers;

public class ShiftController : Controller
{
    private readonly IShiftService _shiftService;

    public ShiftController(IShiftService shiftService)
    {
        _shiftService = shiftService;
    }

    public async Task<IActionResult> Index()
    {
        var shifts = await _shiftService.GetAllShiftsAsync();
        return View(shifts);
    }

    public async Task<IActionResult> Details(long id)
    {
        var shift = await _shiftService.GetShiftByIdAsync(id);
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
        var shift = await _shiftService.GetShiftByIdAsync(id);
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
}
