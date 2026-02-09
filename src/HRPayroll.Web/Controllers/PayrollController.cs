using HRPayroll.Domain.Dtos;
using HRPayroll.Domain.Entities.Payroll;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRPayroll.Web.Controllers
{
    public class PayrollController : Controller
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        // GET: Payroll
        public async Task<IActionResult> Index()
        {
            var payrollRuns = await _payrollService.GetAllPayrollRunsAsync();
            return View(payrollRuns.OrderByDescending(p => p.Year).ThenByDescending(p => p.Month));
        }

        // GET: Payroll/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var payrollRun = await _payrollService.GetPayrollRunByIdAsync(id);
            if (payrollRun == null)
            {
                return NotFound();
            }

            var details = await _payrollService.GetPayrollDetailsAsync(id);
            ViewBag.PayrollDetails = details;
            return View(payrollRun);
        }

        // GET: Payroll/Create
        public IActionResult Create()
        {
            ViewBag.MonthList = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                ViewBag.MonthList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i)
                });
            }
            ViewBag.YearList = new List<SelectListItem>();
            for (int i = DateTime.Now.Year - 1; i <= DateTime.Now.Year + 1; i++)
            {
                ViewBag.YearList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }
            return View();
        }

        // POST: Payroll/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PayrollRun payrollRun)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    payrollRun.Name = $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(payrollRun.Month)} {payrollRun.Year} Payroll";
                    await _payrollService.CreatePayrollRunAsync(payrollRun);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.MonthList = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                ViewBag.MonthList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i),
                    Selected = i == payrollRun.Month
                });
            }
            ViewBag.YearList = new List<SelectListItem>();
            for (int i = DateTime.Now.Year - 1; i <= DateTime.Now.Year + 1; i++)
            {
                ViewBag.YearList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString(),
                    Selected = i == payrollRun.Year
                });
            }
            return View(payrollRun);
        }

        // GET: Payroll/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var payrollRun = await _payrollService.GetPayrollRunByIdAsync(id);
            if (payrollRun == null)
            {
                return NotFound();
            }
            if (payrollRun.Status != PayrollRunStatus.Draft)
            {
                return BadRequest("Only draft payroll runs can be edited.");
            }
            ViewBag.MonthList = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                ViewBag.MonthList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i),
                    Selected = i == payrollRun.Month
                });
            }
            ViewBag.YearList = new List<SelectListItem>();
            for (int i = DateTime.Now.Year - 1; i <= DateTime.Now.Year + 1; i++)
            {
                ViewBag.YearList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString(),
                    Selected = i == payrollRun.Year
                });
            }
            return View(payrollRun);
        }

        // POST: Payroll/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, PayrollRun payrollRun)
        {
            if (id != payrollRun.Id)
            {
                return NotFound();
            }

            if (payrollRun.Status != PayrollRunStatus.Draft)
            {
                return BadRequest("Only draft payroll runs can be edited.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _payrollService.UpdatePayrollRunAsync(payrollRun);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(payrollRun);
        }

        // GET: Payroll/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var payrollRun = await _payrollService.GetPayrollRunByIdAsync(id);
            if (payrollRun == null)
            {
                return NotFound();
            }
            return View(payrollRun);
        }

        // POST: Payroll/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            try
            {
                // Note: Delete method doesn't exist in service, need to implement soft delete
                var payrollRun = await _payrollService.GetPayrollRunByIdAsync(id);
                if (payrollRun != null)
                {
                    payrollRun.Status = PayrollRunStatus.Cancelled;
                    await _payrollService.UpdatePayrollRunAsync(payrollRun);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        // GET: Payroll/Approve/5
        public async Task<IActionResult> Approve(long id)
        {
            var payrollRun = await _payrollService.GetPayrollRunByIdAsync(id);
            if (payrollRun == null)
            {
                return NotFound();
            }
            if (payrollRun.Status != PayrollRunStatus.PendingApproval)
            {
                return BadRequest("Only pending payroll runs can be approved.");
            }
            return View(payrollRun);
        }

        // POST: Payroll/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(long id, long approverId)
        {
            try
            {
                await _payrollService.ApprovePayrollRunAsync(id, approverId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var payrollRun = await _payrollService.GetPayrollRunByIdAsync(id);
                return View(payrollRun);
            }
        }

        // GET: Payroll/Process/5
        public async Task<IActionResult> Process(long id)
        {
            var payrollRun = await _payrollService.GetPayrollRunByIdAsync(id);
            if (payrollRun == null)
            {
                return NotFound();
            }
            if (payrollRun.Status != PayrollRunStatus.Approved)
            {
                return BadRequest("Only approved payroll runs can be processed.");
            }
            return View(payrollRun);
        }

        // POST: Payroll/Process/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessConfirmed(long id)
        {
            try
            {
                await _payrollService.ProcessPayrollAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var payrollRun = await _payrollService.GetPayrollRunByIdAsync(id);
                return View("Process", payrollRun);
            }
        }

        // GET: Payroll/MarkAsPaid/5
        public async Task<IActionResult> MarkAsPaid(long id)
        {
            var payrollRun = await _payrollService.GetPayrollRunByIdAsync(id);
            if (payrollRun == null)
            {
                return NotFound();
            }
            if (payrollRun.Status != PayrollRunStatus.Processed)
            {
                return BadRequest("Only processed payroll runs can be marked as paid.");
            }
            return View(payrollRun);
        }

        // POST: Payroll/MarkAsPaid/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaidConfirmed(long id)
        {
            try
            {
                await _payrollService.MarkAsPaidAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var payrollRun = await _payrollService.GetPayrollRunByIdAsync(id);
                return View("MarkAsPaid", payrollRun);
            }
        }

        // GET: Payroll/RunDetails/5
        public async Task<IActionResult> RunDetails(long id)
        {
            var details = await _payrollService.GetPayrollDetailsAsync(id);
            return View(details);
        }

        // GET: Payroll/GeneratePayslip
        public async Task<IActionResult> GeneratePayslip(long? employeeId, int? month, int? year)
        {
            // Populate employee dropdown
            var employees = await _payrollService.GetAllPayrollRunsAsync(); // This will be replaced with employee service
            ViewBag.Employees = new SelectList(Enumerable.Empty<SelectListItem>());
            
            // For now, use a simple list - in production, get from IEmployeeService
            ViewBag.MonthList = GetMonthList();
            ViewBag.YearList = GetYearList();
            
            if (employeeId.HasValue && month.HasValue && year.HasValue)
            {
                var payslip = await _payrollService.GetPayrollDetailByEmployeeAndMonthAsync(
                    employeeId.Value, month.Value, year.Value);
                    
                if (payslip == null)
                {
                    ViewBag.Error = "No payroll record found for the selected criteria.";
                    return View(new PayslipViewModel());
                }
                
                return View(new PayslipViewModel
                {
                    EmployeeId = payslip.EmployeeId,
                    EmployeeName = payslip.EmployeeName,
                    EmployeeCode = payslip.EmployeeCode,
                    Department = payslip.Department,
                    Designation = payslip.Designation,
                    JoinDate = payslip.JoinDate,
                    BankName = payslip.BankName,
                    BankAccountNo = payslip.BankAccountNo,
                    BranchName = payslip.BranchName,
                    Month = payslip.Month,
                    Year = payslip.Year,
                    MonthName = payslip.MonthName,
                    WorkingDays = payslip.WorkingDays,
                    DaysPresent = payslip.DaysPresent,
                    DaysAbsent = payslip.DaysAbsent,
                    BasicSalary = payslip.BasicSalary,
                    HouseRentAllowance = payslip.HouseRentAllowance,
                    TransportAllowance = payslip.TransportAllowance,
                    MedicalAllowance = payslip.MedicalAllowance,
                    OvertimeAmount = payslip.OvertimeAmount,
                    TotalEarnings = payslip.TotalEarnings,
                    ProvidentFund = payslip.ProvidentFund,
                    TaxDeduction = payslip.TaxDeduction,
                    Insurance = payslip.Insurance,
                    OtherDeductions = payslip.OtherDeductions,
                    TotalDeductions = payslip.TotalDeductions,
                    GrossSalary = payslip.GrossSalary,
                    NetSalary = payslip.NetSalary
                });
            }
            
            return View(new PayslipViewModel());
        }

        private List<SelectListItem> GetMonthList()
        {
            var months = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                months.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i)
                });
            }
            return months;
        }

        private List<SelectListItem> GetYearList()
        {
            var years = new List<SelectListItem>();
            for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year + 1; i++)
            {
                years.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }
            return years;
        }
    }
}
