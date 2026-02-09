using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// Onboarding entity for employee onboarding process
/// </summary>
public class Onboarding : AuditableEntity
{
    [Required]
    public long EmployeeId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public OnboardingStatus Status { get; set; } = OnboardingStatus.NotStarted;

    [MaxLength(2000)]
    public string? ChecklistItems { get; set; }

    [MaxLength(2000)]
    public string? CompletedItems { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public long? AssignedTo { get; set; }

    // Navigation properties
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }

    [ForeignKey(nameof(AssignedTo))]
    public virtual Employee? Assignee { get; set; }
}

/// <summary>
/// PerformanceAppraisal entity for employee performance reviews
/// </summary>
public class PerformanceAppraisal : AuditableEntity
{
    [Required]
    public long EmployeeId { get; set; }

    public long? ReviewerId { get; set; }

    [Required]
    public int AppraisalYear { get; set; }

    [Required]
    public DateTime ReviewDate { get; set; }

    public DateTime? NextReviewDate { get; set; }

    public AppraisalStatus Status { get; set; } = AppraisalStatus.NotStarted;

    // Self Assessment
    [Column(TypeName = "decimal(3,2)")]
    public decimal? SelfRating { get; set; }

    [MaxLength(2000)]
    public string? SelfComments { get; set; }

    [MaxLength(2000)]
    public string? SelfAchievements { get; set; }

    [MaxLength(2000)]
    public string? SelfGoals { get; set; }

    // Manager Assessment
    [Column(TypeName = "decimal(3,2)")]
    public decimal? ManagerRating { get; set; }

    [MaxLength(2000)]
    public string? ManagerComments { get; set; }

    [MaxLength(2000)]
    public string? ManagerAchievements { get; set; }

    [MaxLength(2000)]
    public string? ManagerImprovements { get; set; }

    [MaxLength(2000)]
    public string? ManagerGoals { get; set; }

    // HR Review
    [Column(TypeName = "decimal(3,2)")]
    public decimal? FinalRating { get; set; }

    [MaxLength(2000)]
    public string? HRComments { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? SalaryIncrementPercentage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? BonusPercentage { get; set; }

    public bool IsPromotionRecommended { get; set; } = false;

    public long? PromotionDesignationId { get; set; }

    // Navigation properties
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }

    [ForeignKey(nameof(ReviewerId))]
    public virtual Employee? Reviewer { get; set; }

    [ForeignKey(nameof(PromotionDesignationId))]
    public virtual Designation? PromotionDesignation { get; set; }
}
