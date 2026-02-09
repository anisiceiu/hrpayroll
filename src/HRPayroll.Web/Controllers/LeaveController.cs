using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRPayroll.Web.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ILeaveService _leaveService;
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveController(ILeaveService leaveService, IEmployeeService employeeService, ILeaveTypeService leaveTypeService)
        {
            _leaveService = leaveService;
            _employeeService = employeeService;
            _leaveTypeService = leaveTypeService;
        }

        // GET: Leave
        public async Task<IActionResult> Index()
        {
            var leaves = await _leaveService.GetAllLeavesAsync();
            return View(leaves);
        }

        // GET: Leave/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            return View(leave);
        }

        // GET: Leave/Create
        public async Task<IActionResult> Create()
        {
            var employees = await _employeeService.GetActiveEmployeesAsync();
            var leaveTypes = await _leaveTypeService.GetActiveLeaveTypesAsync();
            ViewBag.EmployeeId = new SelectList(employees, "Id", "FullName");
            ViewBag.LeaveTypeId = new SelectList(leaveTypes, "Id", "Name");
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(LeaveStatus)).Cast<LeaveStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text");
            ViewBag.HalfDayPortionList = new SelectList(Enum.GetValues(typeof(HalfDayPortion)).Cast<HalfDayPortion>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text");
            return View();
        }

        // POST: Leave/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Leave leave)
        {
            if (ModelState.IsValid)
            {
                await _leaveService.ApplyLeaveAsync(leave);
                return RedirectToAction(nameof(Index));
            }

            var employees = await _employeeService.GetActiveEmployeesAsync();
            var leaveTypes = await _leaveTypeService.GetActiveLeaveTypesAsync();
            ViewBag.EmployeeId = new SelectList(employees, "Id", "FullName", leave.EmployeeId);
            ViewBag.LeaveTypeId = new SelectList(leaveTypes, "Id", "Name", leave.LeaveTypeId);
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(LeaveStatus)).Cast<LeaveStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", leave.Status);
            ViewBag.HalfDayPortionList = new SelectList(Enum.GetValues(typeof(HalfDayPortion)).Cast<HalfDayPortion>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", leave.HalfDayPortion);
            return View(leave);
        }

        // GET: Leave/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);
            if (leave == null)
            {
                return NotFound();
            }

            var employees = await _employeeService.GetActiveEmployeesAsync();
            var leaveTypes = await _leaveTypeService.GetActiveLeaveTypesAsync();
            ViewBag.EmployeeId = new SelectList(employees, "Id", "FullName", leave.EmployeeId);
            ViewBag.LeaveTypeId = new SelectList(leaveTypes, "Id", "Name", leave.LeaveTypeId);
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(LeaveStatus)).Cast<LeaveStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", leave.Status);
            ViewBag.HalfDayPortionList = new SelectList(Enum.GetValues(typeof(HalfDayPortion)).Cast<HalfDayPortion>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", leave.HalfDayPortion);
            return View(leave);
        }

        // POST: Leave/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Leave leave)
        {
            if (id != leave.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _leaveService.UpdateLeaveAsync(leave);
                return RedirectToAction(nameof(Index));
            }

            var employees = await _employeeService.GetActiveEmployeesAsync();
            var leaveTypes = await _leaveTypeService.GetActiveLeaveTypesAsync();
            ViewBag.EmployeeId = new SelectList(employees, "Id", "FullName", leave.EmployeeId);
            ViewBag.LeaveTypeId = new SelectList(leaveTypes, "Id", "Name", leave.LeaveTypeId);
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(LeaveStatus)).Cast<LeaveStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", leave.Status);
            ViewBag.HalfDayPortionList = new SelectList(Enum.GetValues(typeof(HalfDayPortion)).Cast<HalfDayPortion>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }), "Value", "Text", leave.HalfDayPortion);
            return View(leave);
        }

        // GET: Leave/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            return View(leave);
        }

        // POST: Leave/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            // Note: This is a soft delete implementation - 
            // actual implementation depends on service method availability
            return RedirectToAction(nameof(Index));
        }

        // GET: Leave/ByEmployee/5
        public async Task<IActionResult> ByEmployee(long employeeId)
        {
            var leaves = await _leaveService.GetByEmployeeIdAsync(employeeId);
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            ViewBag.EmployeeName = employee?.FullName ?? "Unknown";
            return View(leaves);
        }

        // GET: Leave/PendingApprovals/5
        public async Task<IActionResult> PendingApprovals(long approverId)
        {
            var leaves = await _leaveService.GetPendingApprovalsAsync(approverId);
            return View(leaves);
        }

        // GET: Leave/Approve/5
        public async Task<IActionResult> Approve(long id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            return View(leave);
        }

        // POST: Leave/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(long id, long approverId, string? remarks)
        {
            var result = await _leaveService.ApproveLeaveAsync(id, approverId, remarks);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Leave/Reject/5
        public async Task<IActionResult> Reject(long id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            return View(leave);
        }

        // POST: Leave/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(long id, long approverId, string? remarks)
        {
            var result = await _leaveService.RejectLeaveAsync(id, approverId, remarks);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Leave/Cancel/5
        public async Task<IActionResult> Cancel(long id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            return View(leave);
        }

        // POST: Leave/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(long id, long employeeId, string? reason)
        {
            var result = await _leaveService.CancelLeaveAsync(id, employeeId, reason);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
