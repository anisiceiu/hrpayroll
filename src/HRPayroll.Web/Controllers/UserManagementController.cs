using HRPayroll.Domain.Identity;
using HRPayroll.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRPayroll.Web.Controllers;

[Authorize(Roles = "SystemAdmin,Admin")]
public class UserManagementController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<UserManagementController> _logger;

    public UserManagementController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IEmployeeRepository employeeRepository,
        ILogger<UserManagementController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    /// <summary>
    /// List all users with their linked employees
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var userList = new List<UserListViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            string? employeeName = null;
            
            if (user.EmployeeId.HasValue)
            {
                var employee = await _employeeRepository.GetByIdAsync(user.EmployeeId.Value);
                employeeName = employee?.FullName;
            }

            userList.Add(new UserListViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                FullName = user.FullName,
                Role = user.Role.ToString(),
                EmployeeId = user.EmployeeId,
                EmployeeName = employeeName,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate
            });
        }

        return View(userList);
    }

    /// <summary>
    /// Show link employee form for a specific user
    /// </summary>
    public async Task<IActionResult> LinkEmployee(long id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        // Get employees not already linked to other users
        var allEmployees = await _employeeRepository.GetAllWithIncludesAsync();
        var allUsers = _userManager.Users.ToList();
        var linkedEmployeeIds = allUsers.Where(u => u.EmployeeId.HasValue).Select(u => u.EmployeeId!.Value).ToHashSet();
        
        var availableEmployees = allEmployees
            .Where(e => !linkedEmployeeIds.Contains(e.Id) || e.Id == user.EmployeeId)
            .Select(e => new 
            { 
                Id = e.Id, 
                DisplayName = $"{e.FullName} ({e.EmployeeCode})" 
            })
            .ToList();

        var model = new LinkEmployeeViewModel
        {
            UserId = user.Id,
            UserName = user.UserName ?? "",
            UserEmail = user.Email ?? "",
            CurrentEmployeeId = user.EmployeeId,
            AvailableEmployees = new SelectList(availableEmployees, "Id", "DisplayName")
        };

        return View(model);
    }

    /// <summary>
    /// Link employee to user
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LinkEmployee(LinkEmployeeViewModel model)
    {
        
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByIdAsync(model.UserId.ToString());
        if (user == null)
        {
            return NotFound();
        }

        var allEmployees = await _employeeRepository.GetAllWithIncludesAsync();
        var allUsers = _userManager.Users.ToList();
        var linkedEmployeeIds = allUsers.Where(u => u.EmployeeId.HasValue).Select(u => u.EmployeeId!.Value).ToHashSet();
        var availableEmployees = allEmployees
            .Where(e => !linkedEmployeeIds.Contains(e.Id) || e.Id == user.EmployeeId)
            .Select(e => new
            {
                Id = e.Id,
                DisplayName = $"{e.FullName} ({e.EmployeeCode})"
            })
            .ToList();

        model.AvailableEmployees = new SelectList(availableEmployees, "Id", "DisplayName");

        user.EmployeeId = model.SelectedEmployeeId;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            _logger.LogInformation("Admin linked employee {EmployeeId} to user {UserId}", 
                model.SelectedEmployeeId, model.UserId);
            
            TempData["Success"] = "Employee linked successfully";
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    /// <summary>
    /// Unlink employee from user
    /// </summary>
    public async Task<IActionResult> UnlinkEmployee(long id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        user.EmployeeId = null;
        await _userManager.UpdateAsync(user);

        TempData["Success"] = "Employee unlinked successfully";
        return RedirectToAction(nameof(Index));
    }
}

/// <summary>
/// ViewModel for user list
/// </summary>
public class UserListViewModel
{
    public long Id { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Role { get; set; } = "";
    public long? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginDate { get; set; }
}

/// <summary>
/// ViewModel for linking employee
/// </summary>
public class LinkEmployeeViewModel
{
    public long UserId { get; set; }
    public string UserName { get; set; } = "";
    public string UserEmail { get; set; } = "";
    public long? CurrentEmployeeId { get; set; }
    public long? SelectedEmployeeId { get; set; }
    public SelectList? AvailableEmployees { get; set; } = null!;
}
