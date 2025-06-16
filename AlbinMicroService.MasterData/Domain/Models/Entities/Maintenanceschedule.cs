using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbinMicroService.MasterData.Domain.Models.Entities;

/// <summary>
/// Table to schedule maintenance mode windows
/// </summary>
[Table("maintenanceschedules")]
public partial class Maintenanceschedule
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Scheduled start time for maintenance
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Scheduled end time for maintenance
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Reason for maintenance
    /// </summary>
    [Column(TypeName = "text")]
    public string? Reason { get; set; }

    /// <summary>
    /// Is the maintenance window currently active?
    /// </summary>
    [Required]
    public bool? IsActive { get; set; }

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
