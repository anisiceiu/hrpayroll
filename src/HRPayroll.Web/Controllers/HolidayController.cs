using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRPayroll.Web.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class HolidayController : Controller
    {
        private readonly IHolidayService _holidayService;

        public HolidayController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        // GET: Holiday
        public async Task<IActionResult> Index()
        {
            var holidays = await _holidayService.GetAllHolidaysAsync();
            return View(holidays);
        }

        // GET: Holiday/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var holiday = await _holidayService.GetHolidayByIdAsync(id);
            if (holiday == null)
            {
                return NotFound();
            }
            return View(holiday);
        }

        // GET: Holiday/Create
        public IActionResult Create()
        {
            ViewBag.TypeList = new SelectList(Enum.GetValues(typeof(HolidayType)).Cast<HolidayType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text");
            return View();
        }

        // POST: Holiday/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Set DayOfWeek based on the date
                    holiday.DayOfWeek = holiday.Date.ToString("dddd");

                    await _holidayService.CreateHolidayAsync(holiday);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.TypeList = new SelectList(Enum.GetValues(typeof(HolidayType)).Cast<HolidayType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", holiday.Type);
            return View(holiday);
        }

        // GET: Holiday/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var holiday = await _holidayService.GetHolidayByIdAsync(id);
            if (holiday == null)
            {
                return NotFound();
            }
            ViewBag.TypeList = new SelectList(Enum.GetValues(typeof(HolidayType)).Cast<HolidayType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", holiday.Type);
            return View(holiday);
        }

        // POST: Holiday/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Holiday holiday)
        {
            if (id != holiday.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _holidayService.UpdateHolidayAsync(holiday);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.TypeList = new SelectList(Enum.GetValues(typeof(HolidayType)).Cast<HolidayType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", holiday.Type);
            return View(holiday);
        }

        // GET: Holiday/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var holiday = await _holidayService.GetHolidayByIdAsync(id);
            if (holiday == null)
            {
                return NotFound();
            }
            return View(holiday);
        }

        // POST: Holiday/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            try
            {
                await _holidayService.DeleteHolidayAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        // GET: Holiday/ByYear/2024
        public async Task<IActionResult> ByYear(int year)
        {
            var holidays = await _holidayService.GetByYearAsync(year);
            ViewBag.Year = year;
            return View(holidays);
        }
    }
}
