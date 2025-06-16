using System;
using System.Collections.Generic;
using AlbinMicroService.MasterData.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlbinMicroService.MasterData.Domain.ContextDb;

public partial class MasterDataDbContext : DbContext
{
    public MasterDataDbContext(DbContextOptions<MasterDataDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accesslevel> Accesslevels { get; set; }

    public virtual DbSet<Apiversion> Apiversions { get; set; }

    public virtual DbSet<Contacttype> Contacttypes { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Datasourcetype> Datasourcetypes { get; set; }

    public virtual DbSet<Emailtemplate> Emailtemplates { get; set; }

    public virtual DbSet<Featureflag> Featureflags { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Maintenanceschedule> Maintenanceschedules { get; set; }

    public virtual DbSet<Notificationtype> Notificationtypes { get; set; }

    public virtual DbSet<Reporttype> Reporttypes { get; set; }

    public virtual DbSet<Securityquestion> Securityquestions { get; set; }

    public virtual DbSet<Smstemplate> Smstemplates { get; set; }

    public virtual DbSet<Statuscode> Statuscodes { get; set; }

    public virtual DbSet<Systemsetting> Systemsettings { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Accesslevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("accesslevels", tb => tb.HasComment("Master table for access levels"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.Description).HasComment("Permissions or limitations for this access level");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Name).HasComment("Access level name (e.g., Read, Write)");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Apiversion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("apiversions", tb => tb.HasComment("Master table for API version info"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.IsCurrent).HasComment("Indicates if this is the current version");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.ReleaseDate).HasComment("Date of release for this version");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
            entity.Property(e => e.Version).HasComment("API version (e.g., v1, v2)");
        });

        modelBuilder.Entity<Contacttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("contacttypes", tb => tb.HasComment("Master table for contact types"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.Description).HasComment("Description of the contact type");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Name).HasComment("Type of contact (e.g., Mobile, Email)");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("countries", tb => tb.HasComment("Master table for countries"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.Code).HasComment("Country code (e.g., IN, US)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.DialCode).HasComment("Country dial code (e.g., +91)");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Name).HasComment("Country name");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Datasourcetype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("datasourcetypes", tb => tb.HasComment("Master table for different types of data sources"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.Description).HasComment("Additional information about the source type");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Name).HasComment("Name of the data source type (e.g., SQL, Excel)");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Emailtemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("emailtemplates", tb => tb.HasComment("Master table for email templates"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.Body).HasComment("HTML or text body of the email");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Name).HasComment("Template name");
            entity.Property(e => e.Subject).HasComment("Email subject line");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Featureflag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("featureflags", tb => tb.HasComment("Master table for feature flags and toggles"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.Description).HasComment("Description or usage of the feature flag");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.IsEnabled).HasComment("Feature toggle state");
            entity.Property(e => e.KeyName).HasComment("Unique feature flag name/key");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("genders", tb => tb.HasComment("Master table for genders"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.Code).HasComment("Gender code (e.g., M, F, O)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Name).HasComment("Gender name (e.g., Male, Female, Other)");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("languages", tb => tb.HasComment("Master table for supported languages"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.Code).HasComment("Language code (e.g., en)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Name).HasComment("Language name (e.g., English)");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Maintenanceschedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("maintenanceschedules", tb => tb.HasComment("Table to schedule maintenance mode windows"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.EndTime).HasComment("Scheduled end time for maintenance");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasComment("Is the maintenance window currently active?");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Reason).HasComment("Reason for maintenance");
            entity.Property(e => e.StartTime).HasComment("Scheduled start time for maintenance");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Notificationtype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("notificationtypes", tb => tb.HasComment("Master table for notification types"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.Description).HasComment("Details about this notification type");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.TypeName).HasComment("Notification type name (e.g., Email, Push)");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Reporttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reporttypes", tb => tb.HasComment("Master table for report types"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.Description).HasComment("Description or usage of this report type");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Name).HasComment("Name of the report type");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Securityquestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("securityquestions", tb => tb.HasComment("Master table for user security questions"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Question).HasComment("Security question text");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Smstemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("smstemplates", tb => tb.HasComment("Master table for SMS templates"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Message).HasComment("SMS body content");
            entity.Property(e => e.Name).HasComment("Template name");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Statuscode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("statuscodes", tb => tb.HasComment("Master table for various status codes"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.Code).HasComment("Status code (e.g., ACTIVE, INACTIVE)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.Description).HasComment("Detailed description of the status");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        modelBuilder.Entity<Systemsetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("systemsettings", tb => tb.HasComment("Master table for configurable system settings"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.Description).HasComment("Optional description");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.KeyName).HasComment("Unique setting key");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
            entity.Property(e => e.Value).HasComment("Setting value");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("userroles", tb => tb.HasComment("Master table for application user roles"));

            entity.Property(e => e.Id).HasComment("Primary key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was created");
            entity.Property(e => e.CreatedBy).HasComment("User ID who created the record");
            entity.Property(e => e.DeletedAt).HasComment("The datetime when the record was soft-deleted");
            entity.Property(e => e.DeletedBy).HasComment("User ID who deleted the record");
            entity.Property(e => e.Description).HasComment("Description of the user role");
            entity.Property(e => e.IsDeleted).HasComment("Soft delete flag");
            entity.Property(e => e.Name).HasComment("Role name (e.g., Admin, User)");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("The datetime when the record was last updated");
            entity.Property(e => e.UpdatedBy).HasComment("User ID who last updated the record");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
