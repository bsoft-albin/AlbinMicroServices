using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbinMicroService.MasterData.Domain.Models.Entities;

/// <summary>
/// Master table for email templates
/// </summary>
[Table("emailtemplates")]
public partial class Emailtemplate
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Template name
    /// </summary>
    [StringLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Email subject line
    /// </summary>
    [StringLength(255)]
    public string Subject { get; set; } = null!;

    /// <summary>
    /// HTML or text body of the email
    /// </summary>
    [Column(TypeName = "text")]
    public string Body { get; set; } = null!;

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
