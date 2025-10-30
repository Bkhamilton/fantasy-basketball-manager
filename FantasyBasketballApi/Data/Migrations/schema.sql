IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [NbaGames] (
    [Id] int NOT NULL IDENTITY,
    [HomeTeam] nvarchar(10) NOT NULL,
    [AwayTeam] nvarchar(10) NOT NULL,
    [GameTime] datetime2 NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_NbaGames] PRIMARY KEY ([Id])
);

CREATE TABLE [Players] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Team] nvarchar(10) NOT NULL,
    [Position] nvarchar(10) NOT NULL,
    [IsStarting] bit NOT NULL,
    [Stats_Points] decimal(5,2) NULL,
    [Stats_Rebounds] decimal(5,2) NULL,
    [Stats_Assists] decimal(5,2) NULL,
    [Stats_Steals] decimal(5,2) NULL,
    [Stats_Blocks] decimal(5,2) NULL,
    [Stats_FieldGoalPercentage] decimal(5,2) NULL,
    [Stats_ThreePointPercentage] decimal(5,2) NULL,
    [Injury_Status] nvarchar(50) NULL,
    [Injury_Description] nvarchar(500) NULL,
    [GameToday_HasGame] bit NULL,
    [GameToday_Opponent] nvarchar(10) NULL,
    [GameToday_Time] nvarchar(20) NULL,
    [GameToday_IsHomeGame] bit NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251030000205_InitialCreate', N'9.0.10');

COMMIT;
GO

