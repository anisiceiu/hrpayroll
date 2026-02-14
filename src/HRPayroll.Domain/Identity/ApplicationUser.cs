using Microsoft.AspNetCore.Identity;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Identity;

/// <summary>
/// Application user for authentication - extends IdentityUser
/// </summary>
public class ApplicationUser : IdentityUser<long>
{
    /// <summary>
    /// First name of the user
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name of the user
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the user
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Employee ID if this user is linked to an employee
    /// </summary>
    public long? EmployeeId { get; set; }

    /// <summary>
    /// User role for authorization
    /// </summary>
    public UserRole Role { get; set; } = UserRole.Employee;

    /// <summary>
    /// Profile photo path
    /// </summary>
    public string? ProfilePhotoPath { get; set; }

    /// <summary>
    /// Is the user active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Last login date
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Update date
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
