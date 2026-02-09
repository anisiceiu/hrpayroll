using System.ComponentModel.DataAnnotations;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Enums;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// Holiday entity for managing organizational holidays
/// </summary>
public class Holiday : AuditableEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? NameBN { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [MaxLength(10)]
    public string? DayOfWeek { get; set; }

    [Required]
    public HolidayType Type { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsRepeatAnnually { get; set; } = false;

    [MaxLength(50)]
    public string? Country { get; set; } = "Bangladesh";
}
