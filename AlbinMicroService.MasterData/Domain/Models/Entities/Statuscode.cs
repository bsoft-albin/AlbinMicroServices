using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AlbinMicroService.MasterData.Domain.Models.Entities;

/// <summary>
/// Master table for various status codes
/// </summary>
[Table("statuscodes")]
[Index("Code", Name = "idx_statuscodes_code")]
public partial class Statuscode
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Status code (e.g., ACTIVE, INACTIVE)
    /// </summary>
    [StringLength(50)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// Detailed description of the status
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
