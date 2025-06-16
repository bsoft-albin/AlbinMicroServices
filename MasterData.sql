CREATE SCHEMA IF NOT EXISTS master_data;

USE DATABASE master_data;

-- 1. Languages
CREATE TABLE Languages (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Name VARCHAR(100) NOT NULL COMMENT 'Language name (e.g., English)',
    Code VARCHAR(10) NOT NULL COMMENT 'Language code (e.g., en)',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for supported languages';

CREATE INDEX idx_languages_code ON Languages (Code);

-- 2. Countries
CREATE TABLE Countries (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Name VARCHAR(100) NOT NULL COMMENT 'Country name',
    Code VARCHAR(10) NOT NULL COMMENT 'Country code (e.g., IN, US)',
    DialCode VARCHAR(10) COMMENT 'Country dial code (e.g., +91)',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for countries';

CREATE INDEX idx_countries_code ON Countries (Code);

-- 3. Genders
CREATE TABLE Genders (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Name VARCHAR(50) NOT NULL COMMENT 'Gender name (e.g., Male, Female, Other)',
    Code VARCHAR(10) NOT NULL COMMENT 'Gender code (e.g., M, F, O)',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for genders';

CREATE INDEX idx_genders_code ON Genders (Code);

-- 4. StatusCodes
CREATE TABLE StatusCodes (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Code VARCHAR(50) NOT NULL COMMENT 'Status code (e.g., ACTIVE, INACTIVE)',
    Description TEXT COMMENT 'Detailed description of the status',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for various status codes';

CREATE INDEX idx_statuscodes_code ON StatusCodes (Code);

-- 5. UserRoles
CREATE TABLE UserRoles (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Name VARCHAR(100) NOT NULL COMMENT 'Role name (e.g., Admin, User)',
    Description TEXT COMMENT 'Description of the user role',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for application user roles';

-- 6. ContactTypes
CREATE TABLE ContactTypes (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Name VARCHAR(100) NOT NULL COMMENT 'Type of contact (e.g., Mobile, Email)',
    Description TEXT COMMENT 'Description of the contact type',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for contact types';

-- 7. FeatureFlags
CREATE TABLE FeatureFlags (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    KeyName VARCHAR(100) NOT NULL COMMENT 'Unique feature flag name/key',
    IsEnabled BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Feature toggle state',
    Description TEXT COMMENT 'Description or usage of the feature flag',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for feature flags and toggles';

-- 8. SystemSettings
CREATE TABLE SystemSettings (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    KeyName VARCHAR(100) NOT NULL COMMENT 'Unique setting key',
    Value TEXT NOT NULL COMMENT 'Setting value',
    Description TEXT COMMENT 'Optional description',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for configurable system settings';

-- 9. ApiVersions
CREATE TABLE ApiVersions (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Version VARCHAR(20) NOT NULL COMMENT 'API version (e.g., v1, v2)',
    IsCurrent BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Indicates if this is the current version',
    ReleaseDate DATE COMMENT 'Date of release for this version',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for API version info';

-- 10. MaintenanceSchedules
CREATE TABLE MaintenanceSchedules (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    StartTime DATETIME NOT NULL COMMENT 'Scheduled start time for maintenance',
    EndTime DATETIME NOT NULL COMMENT 'Scheduled end time for maintenance',
    Reason TEXT COMMENT 'Reason for maintenance',
    IsActive BOOLEAN NOT NULL DEFAULT TRUE COMMENT 'Is the maintenance window currently active?',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Table to schedule maintenance mode windows';

-- 11. EmailTemplates
CREATE TABLE EmailTemplates (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Name VARCHAR(100) NOT NULL COMMENT 'Template name',
    Subject VARCHAR(255) NOT NULL COMMENT 'Email subject line',
    Body TEXT NOT NULL COMMENT 'HTML or text body of the email',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for email templates';

-- 12. SmsTemplates
CREATE TABLE SmsTemplates (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Name VARCHAR(100) NOT NULL COMMENT 'Template name',
    Message TEXT NOT NULL COMMENT 'SMS body content',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for SMS templates';

-- 13. NotificationTypes
CREATE TABLE NotificationTypes (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    TypeName VARCHAR(100) NOT NULL COMMENT 'Notification type name (e.g., Email, Push)',
    Description TEXT COMMENT 'Details about this notification type',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for notification types';

-- 14. ReportTypes
CREATE TABLE ReportTypes (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Name VARCHAR(100) NOT NULL COMMENT 'Name of the report type',
    Description TEXT COMMENT 'Description or usage of this report type',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for report types';

-- 15. DataSourceTypes
CREATE TABLE DataSourceTypes (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Name VARCHAR(100) NOT NULL COMMENT 'Name of the data source type (e.g., SQL, Excel)',
    Description TEXT COMMENT 'Additional information about the source type',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for different types of data sources';

-- 16. AccessLevels
CREATE TABLE AccessLevels (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Name VARCHAR(100) NOT NULL COMMENT 'Access level name (e.g., Read, Write)',
    Description TEXT COMMENT 'Permissions or limitations for this access level',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for access levels';

-- 17. SecurityQuestions
CREATE TABLE SecurityQuestions (
    Id INT PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary key',
    Question VARCHAR(255) NOT NULL COMMENT 'Security question text',
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime when the record was created',
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the record was last updated',
    DeletedAt DATETIME NULL COMMENT 'The datetime when the record was soft-deleted',
    CreatedBy BIGINT NULL COMMENT 'User ID who created the record',
    UpdatedBy BIGINT NULL COMMENT 'User ID who last updated the record',
    DeletedBy BIGINT NULL COMMENT 'User ID who deleted the record',
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Soft delete flag'
) COMMENT = 'Master table for user security questions';

---- Sample Insert Queries for the Above Tables ----
INSERT INTO Languages (Name, Code) VALUES
('English', 'en'),
('Spanish', 'es'),
('Hindi', 'hi');
INSERT INTO Countries (Name, Code, DialCode) VALUES
('India', 'IN', '+91'),
('United States', 'US', '+1'),
('Germany', 'DE', '+49');
INSERT INTO Genders (Name, Code) VALUES
('Male', 'M'),
('Female', 'F'),
('Other', 'O');
INSERT INTO StatusCodes (Code, Description) VALUES
('ACTIVE', 'Record is active'),
('INACTIVE', 'Record is inactive'),
('PENDING', 'Record is pending approval');
INSERT INTO UserRoles (Name, Description) VALUES
('Admin', 'System administrator with full access'),
('User', 'Standard application user'),
('Manager', 'Manages a group of users');
INSERT INTO ContactTypes (Name, Description) VALUES
('Email', 'Email address'),
('Mobile', 'Mobile phone number'),
('Fax', 'Fax number');
INSERT INTO FeatureFlags (KeyName, IsEnabled, Description) VALUES
('NewDashboard', TRUE, 'Enable the new dashboard design'),
('EnableChat', FALSE, 'Enable in-app chat support');
INSERT INTO SystemSettings (KeyName, Value, Description) VALUES
('AppName', 'Albin MicroServices', 'Albin Micro-Services Inc'),
('MaxUploadSize', '50', 'Max upload file size in MB');
INSERT INTO ApiVersions (Version, IsCurrent, ReleaseDate) VALUES
('v1', TRUE, '2023-01-01'),
('v2', FALSE, '2024-01-01');
INSERT INTO MaintenanceSchedules (StartTime, EndTime, Reason, IsActive) VALUES
('2025-07-01 00:00:00', '2025-07-01 02:00:00', 'Database upgrade', FALSE),
('2025-12-25 01:00:00', '2025-12-25 03:00:00', 'Christmas maintenance window', TRUE);
INSERT INTO EmailTemplates (Name, Subject, Body) VALUES
('WelcomeEmail', 'Welcome to Our Service', '<h1>Welcome!</h1><p>Thanks for joining us.</p>'),
('PasswordReset', 'Reset Your Password', '<p>Click the link below to reset your password.</p>');
INSERT INTO SmsTemplates (Name, Message) VALUES
('OTP', 'Your OTP code is {{code}}'),
('PasswordReset', 'Reset your password using this link: {{link}}');
INSERT INTO NotificationTypes (TypeName, Description) VALUES
('Email', 'Email notification'),
('Push', 'Mobile push notification'),
('SMS', 'Text message alert');
INSERT INTO ReportTypes (Name, Description) VALUES
('SalesReport', 'Report showing sales performance'),
('UserActivity', 'Report on user activity logs');
INSERT INTO DataSourceTypes (Name, Description) VALUES
('SQL', 'MySQL Or PostgreSQl databases'),
('Excel', 'Microsoft Excel spreadsheets'),
('CSV', 'Comma-separated value files');
INSERT INTO AccessLevels (Name, Description) VALUES
('ReadOnly', 'Can only view data'),
('ReadWrite', 'Can view and modify data'),
('Admin', 'Full access including deletion');
INSERT INTO SecurityQuestions (Question) VALUES
('What is your mother’s maiden name?'),
('What was your first pet’s name?'),
('What was the name of your elementary school?');


-- Scafffold Command of Mysql -from the Root of the Project ----
-- dotnet ef dbcontext scaffold "Server=localhost;Port=3306;Database=master_data;Uid=root;Pwd=albin;" Pomelo.EntityFrameworkCore.MySql --context MasterDataDbContext --output-dir Domain/Models/Entities --context-dir Domain/ContextDb --data-annotations --no-onconfiguring --project . --startup-project . --force
