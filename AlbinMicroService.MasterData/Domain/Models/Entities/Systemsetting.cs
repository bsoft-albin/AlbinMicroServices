using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbinMicroService.MasterData.Domain.Models.Entities;

/// <summary>
/// Master table for configurable system settings
/// </summary>
[Table("systemsettings")]
public partial class Systemsetting
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Unique setting key
    /// </summary>
    [StringLength(100)]
    public string KeyName { get; set; } = null!;

    /// <summary>
    /// Setting value
    /// </summary>
    [Column(TypeName = "text")]
    public string Value { get; set; } = null!;

    /// <summary>
    /// Optional description
    /// </summary>
    [Column(TypeName = "text")]
    public string? Description { get; set; }

    /// <summary>
    /// The datetime when the record was created
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The datetime when the record was last updated
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// The datetime when the record was soft-deleted
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// User ID who created the record
    /// </summary>
    public long? CreatedBy { get; set; }

    /// <summary>
    /// User ID who last updated the record
    /// </summary>
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// User ID who deleted the record
    /// </summary>
    public long? DeletedBy { get; set; }

    /// <summary>
    /// Soft delete flag
    /// </summary>
    public bool IsDeleted { get; set; }
}
