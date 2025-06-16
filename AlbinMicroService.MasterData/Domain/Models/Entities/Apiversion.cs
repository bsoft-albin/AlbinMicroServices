using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbinMicroService.MasterData.Domain.Models.Entities;

/// <summary>
/// Master table for API version info
/// </summary>
[Table("apiversions")]
public partial class Apiversion
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// API version (e.g., v1, v2)
    /// </summary>
    [StringLength(20)]
    public string Version { get; set; } = null!;

    /// <summary>
    /// Indicates if this is the current version
    /// </summary>
    public bool IsCurrent { get; set; }

    /// <summary>
    /// Date of release for this version
    /// </summary>
    public DateOnly? ReleaseDate { get; set; }

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
