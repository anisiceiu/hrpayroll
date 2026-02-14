using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// EmployeeDocument entity for employee documents and files
/// </summary>
public class EmployeeDocument : AuditableEntity
{
    [Required]
    public long EmployeeId { get; set; }

    /// <summary>
    /// Category ID for the document
    /// </summary>
    public long? CategoryId { get; set; }

    [Required]
    [MaxLength(100)]
    public string DocumentName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? DocumentType { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(255)]
    public string FilePath { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FileName { get; set; }

    [MaxLength(50)]
    public string? FileExtension { get; set; }

    /// <summary>
    /// File size in bytes
    /// </summary>
    public long? FileSize { get; set; }

    /// <summary>
    /// MIME content type
    /// </summary>
    [MaxLength(100)]
    public string? ContentType { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool IsVerified { get; set; } = false;

    public long? VerifiedBy { get; set; }

    public DateTime? VerifiedDate { get; set; }
    public DateTime? CreatedDate { get; set; }

    // Navigation properties
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual DocumentCategory? Category { get; set; }

    [ForeignKey(nameof(VerifiedBy))]
    public virtual Employee? Verifier { get; set; }
}
