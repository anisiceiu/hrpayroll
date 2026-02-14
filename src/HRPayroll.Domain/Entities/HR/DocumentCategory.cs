using System.ComponentModel.DataAnnotations;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// DocumentCategory entity for organizing document types
/// </summary>
public class DocumentCategory : AuditableEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? NameBN { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Display order in lists
    /// </summary>
    public int DisplayOrder { get; set; } = 0;
    public DateTime? CreatedDate { get; set; }
    /// <summary>
    /// Whether this category is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Whether documents in this category can expire
    /// </summary>
    public bool HasExpiryDate { get; set; } = false;

    /// <summary>
    /// Default file size limit in MB for this category
    /// </summary>
    public int? MaxFileSizeMB { get; set; }

    // Navigation properties
    public virtual ICollection<EmployeeDocument> Documents { get; set; } = new List<EmployeeDocument>();
}
