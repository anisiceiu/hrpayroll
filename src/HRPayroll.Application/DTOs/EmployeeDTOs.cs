using System.ComponentModel.DataAnnotations;
using HRPayroll.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace HRPayroll.Application.DTOs;

/// <summary>
/// Employee DTO for creating/updating employees
/// </summary>
public class EmployeeDTO
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Employee code is required")]
    [MaxLength(20, ErrorMessage = "Employee code cannot exceed 20 characters")]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
    public string? Phone { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public Gender Gender { get; set; } = Gender.Male;

    public MaritalStatus MaritalStatus { get; set; } = MaritalStatus.Single;

    [MaxLength(20, ErrorMessage = "National ID cannot exceed 20 characters")]
    public string? NationalId { get; set; }

    [MaxLength(20, ErrorMessage = "TIN cannot exceed 20 characters")]
    public string? TinNo { get; set; }

    [MaxLength(5, ErrorMessage = "Blood group cannot exceed 5 characters")]
    public string? BloodGroup { get; set; }

    [MaxLength(500, ErrorMessage = "Present address cannot exceed 500 characters")]
    public string? PresentAddress { get; set; }

    [MaxLength(500, ErrorMessage = "Permanent address cannot exceed 500 characters")]
    public string? PermanentAddress { get; set; }

    public IFormFile? ProfilePhotoFile { get; set; }
    public string? ProfilePhoto { get; set; }

    [Required(ErrorMessage = "Department is required")]
    public long DepartmentId { get; set; }

    [Required(ErrorMessage = "Designation is required")]
    public long DesignationId { get; set; }

    public long? ShiftId { get; set; }

    public long? SupervisorId { get; set; }

    [Required(ErrorMessage = "Date of joining is required")]
    public DateTime DateOfJoining { get; set; }

    public DateTime? DateOfConfirmation { get; set; }

    public EmploymentType EmploymentType { get; set; } = EmploymentType.Permanent;

    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

    public bool IsTaxExempted { get; set; } = false;

    [MaxLength(50, ErrorMessage = "Bank account number cannot exceed 50 characters")]
    public string? BankAccountNo { get; set; }

    [MaxLength(100, ErrorMessage = "Bank name cannot exceed 100 characters")]
    public string? BankName { get; set; }

    [MaxLength(100, ErrorMessage = "Branch name cannot exceed 100 characters")]
    public string? BranchName { get; set; }
}

/// <summary>
/// Employee list DTO for grid display
/// </summary>
public class EmployeeListDTO
{
    public long Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string DesignationName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DateOfJoining { get; set; }
    public string? ProfilePhoto { get; set; }
}

/// <summary>
/// Employee detail DTO for detailed view
/// </summary>
public class EmployeeDetailDTO
{
    public long Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string MaritalStatus { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public string? TinNo { get; set; }
    public string? BloodGroup { get; set; }
    public string? PresentAddress { get; set; }
    public string? PermanentAddress { get; set; }
    public string? ProfilePhoto { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string DesignationName { get; set; } = string.Empty;
    public string? ShiftName { get; set; }
    public string? SupervisorName { get; set; }
    public DateTime DateOfJoining { get; set; }
    public DateTime? DateOfConfirmation { get; set; }
    public string EmploymentType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? BankAccountNo { get; set; }
    public string? BankName { get; set; }
    public string? BranchName { get; set; }
}
