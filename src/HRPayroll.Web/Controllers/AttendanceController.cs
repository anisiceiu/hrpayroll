using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRPayroll.Web.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;

        public AttendanceController(IAttendanceService attendanceService, IEmployeeService employeeService)
        {
            _attendanceService = attendanceService;
            _employeeService = employeeService;
        }

        // GET: Attendance
        public async Task<IActionResult> Index()
        {
            var attendances = await _attendanceService.GetAllAttendancesAsync();
            return View(attendances);
        }

        // GET: Attendance/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        // GET: Attendance/Create
        public async Task<IActionResult> Create()
        {
            var employees = await _employeeService.GetActiveEmployeesAsync();
            ViewBag.EmployeeId = new SelectList(employees, "Id", "FullName");
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(AttendanceStatus)).Cast<AttendanceStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text");
            ViewBag.EntryTypeList = new SelectList(Enum.GetValues(typeof(EntryType)).Cast<EntryType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text");
            return View();
        }

        // POST: Attendance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                // Check if attendance already exists for this employee on this date
                var existing = await _attendanceService.GetByEmployeeAndDateAsync(attendance.EmployeeId, attendance.Date);
                if (existing != null)
                {
                    ModelState.AddModelError("", "Attendance already exists for this employee on this date.");
                    var employees = await _employeeService.GetActiveEmployeesAsync();
                    ViewBag.EmployeeId = new SelectList(employees, "Id", "FullName", attendance.EmployeeId);
                    ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(AttendanceStatus)).Cast<AttendanceStatus>()
                        .Select(e => new SelectListItem
                        {
                            Value = ((int)e).ToString(),
                            Text = e.ToString()
                        }), "Value", "Text", attendance.Status);
                    ViewBag.EntryTypeList = new SelectList(Enum.GetValues(typeof(EntryType)).Cast<EntryType>()
                        .Select(e => new SelectListItem
                        {
                            Value = ((int)e).ToString(),
                            Text = e.ToString()
                        }), "Value", "Text", attendance.EntryType);
                    return View(attendance);
                }

                await _attendanceService.RecordAttendanceAsync(attendance);
                return RedirectToAction(nameof(Index));
            }

            var empList = await _employeeService.GetActiveEmployeesAsync();
            ViewBag.EmployeeId = new SelectList(empList, "Id", "FullName", attendance.EmployeeId);
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(AttendanceStatus)).Cast<AttendanceStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", attendance.Status);
            ViewBag.EntryTypeList = new SelectList(Enum.GetValues(typeof(EntryType)).Cast<EntryType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", attendance.EntryType);
            return View(attendance);
        }

        // GET: Attendance/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            var employees = await _employeeService.GetActiveEmployeesAsync();
            ViewBag.EmployeeId = new SelectList(employees, "Id", "FullName", attendance.EmployeeId);
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(AttendanceStatus)).Cast<AttendanceStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", attendance.Status);
            ViewBag.EntryTypeList = new SelectList(Enum.GetValues(typeof(EntryType)).Cast<EntryType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", attendance.EntryType);
            return View(attendance);
        }

        // POST: Attendance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _attendanceService.UpdateAttendanceAsync(attendance);
                return RedirectToAction(nameof(Index));
            }

            var employees = await _employeeService.GetActiveEmployeesAsync();
            ViewBag.EmployeeId = new SelectList(employees, "Id", "FullName", attendance.EmployeeId);
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(AttendanceStatus)).Cast<AttendanceStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", attendance.Status);
            ViewBag.EntryTypeList = new SelectList(Enum.GetValues(typeof(EntryType)).Cast<EntryType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", attendance.EntryType);
            return View(attendance);
        }

        // GET: Attendance/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        // POST: Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var result = await _attendanceService.DeleteAttendanceAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Attendance/ByEmployee/5
        public async Task<IActionResult> ByEmployee(long employeeId)
        {
            var attendances = await _attendanceService.GetByEmployeeIdAsync(employeeId);
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            ViewBag.EmployeeName = employee?.FullName ?? "Unknown";
            return View(attendances);
        }

        // GET: Attendance/DateRange
        public async Task<IActionResult> DateRange()
        {
            var employees = await _employeeService.GetActiveEmployeesAsync();
            ViewBag.EmployeeId = new SelectList(employees, "Id", "FullName");
            return View();
        }

        // POST: Attendance/DateRange
        [HttpPost]
        public async Task<IActionResult> DateRange(long employeeId, DateTime startDate, DateTime endDate)
        {
            var attendances = await _attendanceService.GetByDateRangeAsync(employeeId, startDate, endDate);
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            ViewBag.EmployeeName = employee?.FullName ?? "Unknown";
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View("ByEmployee", attendances);
        }
    }
}
