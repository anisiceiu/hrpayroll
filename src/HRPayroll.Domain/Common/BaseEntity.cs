using System.ComponentModel.DataAnnotations;

namespace HRPayroll.Domain.Common;

/// <summary>
/// Base entity with common audit properties
/// </summary>
public abstract class BaseEntity
{
    public long Id { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }
}

/// <summary>
/// Entity with soft delete capability
/// </summary>
public abstract class SoftDeleteEntity : BaseEntity
{
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public long? DeletedBy { get; set; }
}

/// <summary>
/// Auditable entity for tracking changes
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    [MaxLength(500)]
    public string? Remarks { get; set; }
}
