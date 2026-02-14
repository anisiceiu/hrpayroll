using Microsoft.AspNetCore.Identity;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Identity;

/// <summary>
/// Application role for authorization - extends IdentityRole
/// </summary>
public class ApplicationRole : IdentityRole<long>
{
    /// <summary>
    /// User role enum
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Role description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Is the role active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Update date
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
