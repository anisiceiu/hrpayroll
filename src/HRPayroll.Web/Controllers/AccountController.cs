using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HRPayroll.Domain.Identity;
using HRPayroll.Web.Models;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        if (!user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "Your account has been deactivated. Please contact administrator.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName ?? model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            // Update last login date
            user.LastLoginDate = DateTime.Now;
            await _userManager.UpdateAsync(user);
            
            _logger.LogInformation("User logged in: {Email}", user.Email);
            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "Account is locked out. Please try again later.");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        ViewBag.Roles = GetAvailableRoles();
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        ViewBag.Roles = GetAvailableRoles();
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError(string.Empty, "Email is already registered.");
            return View(model);
        }

        var existingUserName = await _userManager.FindByNameAsync(model.UserName);
        if (existingUserName != null)
        {
            ModelState.AddModelError(string.Empty, "Username is already taken.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Role = model.Role,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Assign role to user
            var roleName = model.Role.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new ApplicationRole
                {
                    Name = roleName,
                    Role = model.Role,
                    Description = $"User with {model.Role} role",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                await _roleManager.CreateAsync(role);
            }
            
            await _userManager.AddToRoleAsync(user, roleName);
            
            _logger.LogInformation("User registered: {Email} with role {Role}", user.Email, roleName);

            // Auto login after registration
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private List<RoleItem> GetAvailableRoles()
    {
        return new List<RoleItem>
        {
            new RoleItem { Role = UserRole.SystemAdmin, DisplayName = "System Administrator" },
            new RoleItem { Role = UserRole.HRManager, DisplayName = "HR Manager" },
            new RoleItem { Role = UserRole.HRExecutive, DisplayName = "HR Executive" },
            new RoleItem { Role = UserRole.PayrollManager, DisplayName = "Payroll Manager" },
            new RoleItem { Role = UserRole.PayrollExecutive, DisplayName = "Payroll Executive" },
            new RoleItem { Role = UserRole.DepartmentManager, DisplayName = "Department Manager" },
            new RoleItem { Role = UserRole.FinanceManager, DisplayName = "Finance Manager" },
            new RoleItem { Role = UserRole.Employee, DisplayName = "Employee" }
        };
    }
}

public class RoleItem
{
    public UserRole Role { get; set; }
    public string DisplayName { get; set; } = string.Empty;
}
