using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// Recruitment entity for job postings
/// </summary>
public class Recruitment : AuditableEntity
{
    [Required]
    [MaxLength(100)]
    public string JobTitle { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? JobCode { get; set; }

    [Required]
    public long DepartmentId { get; set; }

    [Required]
    public long DesignationId { get; set; }

    public int NumberOfPositions { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MinSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MaxSalary { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [MaxLength(2000)]
    public string? JobDescription { get; set; }

    [MaxLength(2000)]
    public string? Requirements { get; set; }

    [MaxLength(1000)]
    public string? Location { get; set; }

    public EmploymentType EmploymentType { get; set; } = EmploymentType.Permanent;

    public RecruitmentStatus Status { get; set; } = RecruitmentStatus.Open;

    public int? MinimumExperience { get; set; }

    [MaxLength(100)]
    public string? Education { get; set; }

    // Navigation properties
    public virtual Department? Department { get; set; }
    public virtual Designation? Designation { get; set; }
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}

/// <summary>
/// Application entity for job applications
/// </summary>
public class Application : AuditableEntity
{
    [Required]
    public long RecruitmentId { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? CurrentCompany { get; set; }

    [MaxLength(100)]
    public string? CurrentPosition { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? CurrentSalary { get; set; }

    [MaxLength(100)]
    public string? ExpectedSalary { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string? LinkedInProfile { get; set; }

    [MaxLength(255)]
    public string? ResumePath { get; set; }

    [MaxLength(255)]
    public string? CoverLetterPath { get; set; }

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

    public DateTime AppliedDate { get; set; } = DateTime.Now;

    [MaxLength(1000)]
    public string? Notes { get; set; }

    // Navigation properties
    [ForeignKey(nameof(RecruitmentId))]
    public virtual Recruitment? Recruitment { get; set; }
}
