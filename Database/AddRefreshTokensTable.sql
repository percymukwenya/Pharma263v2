-- =============================================
-- Script: Add RefreshTokens Table
-- Description: Creates RefreshTokens table for JWT refresh token functionality
-- Author: Security Enhancement - Phase 2
-- Date: 2025-11-05
-- =============================================

-- Create RefreshTokens table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefreshTokens]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[RefreshTokens]
    (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Token] NVARCHAR(500) NOT NULL,
        [UserId] NVARCHAR(450) NOT NULL,
        [ExpiryDate] DATETIME2(7) NOT NULL,
        [IsRevoked] BIT NOT NULL DEFAULT 0,
        [RevokedDate] DATETIME2(7) NULL,
        [RevokedByIp] NVARCHAR(50) NULL,
        [CreatedByIp] NVARCHAR(50) NULL,
        [ReplacedByToken] NVARCHAR(500) NULL,
        [CreatedDate] DATETIMEOFFSET(7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        [CreatedBy] NVARCHAR(100) NULL,
        [ModifiedDate] DATETIMEOFFSET(7) NULL,
        [ModifiedBy] NVARCHAR(100) NULL,
        [IsDeleted] BIT NOT NULL DEFAULT 0,

        CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_RefreshTokens_AspNetUsers] FOREIGN KEY ([UserId])
            REFERENCES [dbo].[AspNetUsers]([Id]) ON DELETE CASCADE
    );

    -- Create index on Token for faster lookups
    CREATE NONCLUSTERED INDEX [IX_RefreshTokens_Token]
    ON [dbo].[RefreshTokens]([Token] ASC)
    WHERE [IsDeleted] = 0 AND [IsRevoked] = 0;

    -- Create index on UserId for faster user token lookups
    CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId]
    ON [dbo].[RefreshTokens]([UserId] ASC)
    WHERE [IsDeleted] = 0;

    -- Create index on ExpiryDate for cleanup operations
    CREATE NONCLUSTERED INDEX [IX_RefreshTokens_ExpiryDate]
    ON [dbo].[RefreshTokens]([ExpiryDate] ASC)
    WHERE [IsDeleted] = 0;

    PRINT 'RefreshTokens table created successfully.';
END
ELSE
BEGIN
    PRINT 'RefreshTokens table already exists.';
END
GO

-- Grant permissions (adjust as needed for your environment)
-- GRANT SELECT, INSERT, UPDATE, DELETE ON [dbo].[RefreshTokens] TO [YourAppUser];
-- GO

PRINT 'Refresh token database setup complete.';
GO
