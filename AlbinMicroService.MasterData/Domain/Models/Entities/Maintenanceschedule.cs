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
