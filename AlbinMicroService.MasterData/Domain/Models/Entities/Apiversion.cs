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

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public bool IsDeleted { get; set; }
}
