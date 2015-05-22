
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/28/2014 23:35:45
-- Generated from EDMX file: C:\Users\Ljubisa\Documents\Visual Studio 2012\Projects\MvcApplication2\MvcApplication2\EFModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [IgrajKarte];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[fk_LoginSessions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [fk_LoginSessions];
GO
IF OBJECT_ID(N'[dbo].[FK_LoginSessions_LoginSessions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LoginSessions] DROP CONSTRAINT [FK_LoginSessions_LoginSessions];
GO
IF OBJECT_ID(N'[dbo].[fk_PerOrders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [fk_PerOrders];
GO
IF OBJECT_ID(N'[dbo].[fk_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LoginSessions] DROP CONSTRAINT [fk_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[LoginSessions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LoginSessions];
GO
IF OBJECT_ID(N'[dbo].[Logs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Logs];
GO
IF OBJECT_ID(N'[dbo].[Player]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Player];
GO
IF OBJECT_ID(N'[dbo].[Rooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rooms];
GO
IF OBJECT_ID(N'[dbo].[Tablic]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tablic];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'LoginSessions'
CREATE TABLE [dbo].[LoginSessions] (
    [Id] int  NOT NULL,
    [UserId] int  NOT NULL,
    [Token] nvarchar(50)  NULL,
    [DateCreated] nvarchar(50)  NULL,
    [DateModified] nvarchar(50)  NULL
);
GO

-- Creating table 'Logs'
CREATE TABLE [dbo].[Logs] (
    [Id] int  NOT NULL,
    [UserId] int  NOT NULL,
    [SessionId] int  NULL,
    [Type] nvarchar(50)  NULL,
    [Message] nvarchar(50)  NULL,
    [DateCreated] nvarchar(50)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [Address] nvarchar(50)  NULL,
    [City] nvarchar(50)  NULL,
    [Phone] nvarchar(50)  NULL,
    [Level] int  NULL,
    [DateCreated] nvarchar(50)  NULL,
    [DateModified] nvarchar(50)  NULL,
    [IsSuspended] bit  NOT NULL,
    [IsAutenticated] bit  NOT NULL,
    [IsVerified] bit  NOT NULL,
    [UserName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Rooms'
CREATE TABLE [dbo].[Rooms] (
    [GameId] int  NOT NULL,
    [GameCodeName] nchar(15)  NOT NULL,
    [PlayerIDIn] nchar(20)  NULL,
    [PlayerIDOut] nchar(200)  NULL
);
GO

-- Creating table 'Tablics'
CREATE TABLE [dbo].[Tablics] (
    [Id] int  NOT NULL,
    [User1] int  NOT NULL,
    [User2] int  NOT NULL,
    [User3] int  NULL,
    [User4] int  NULL,
    [ActiveUser] int  NOT NULL,
    [Cards] varchar(50)  NULL,
    [CardsInDeck] nvarchar(200)  NULL,
    [Status] int  NOT NULL,
    [NOPlayers] int  NULL,
    [SelectedCards] nvarchar(30)  NULL,
    [AvailablePositionsOnTable] nvarchar(30)  NULL
);
GO

-- Creating table 'Players'
CREATE TABLE [dbo].[Players] (
    [Id] int  NOT NULL,
    [UserId] int  NOT NULL,
    [CardsInHand] nchar(100)  NULL,
    [CardsOnHeap] nchar(100)  NULL,
    [Score] int  NOT NULL,
    [Dots] int  NOT NULL,
    [GameType] int  NOT NULL,
    [Win_Loss] int  NOT NULL,
    [DateCreated] nchar(20)  NULL,
    [DateModified] nchar(20)  NULL,
    [GameId] int  NULL,
    [PlayerStatus] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'LoginSessions'
ALTER TABLE [dbo].[LoginSessions]
ADD CONSTRAINT [PK_LoginSessions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Logs'
ALTER TABLE [dbo].[Logs]
ADD CONSTRAINT [PK_Logs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [GameId] in table 'Rooms'
ALTER TABLE [dbo].[Rooms]
ADD CONSTRAINT [PK_Rooms]
    PRIMARY KEY CLUSTERED ([GameId] ASC);
GO

-- Creating primary key on [Id] in table 'Tablics'
ALTER TABLE [dbo].[Tablics]
ADD CONSTRAINT [PK_Tablics]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Players'
ALTER TABLE [dbo].[Players]
ADD CONSTRAINT [PK_Players]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SessionId] in table 'Logs'
ALTER TABLE [dbo].[Logs]
ADD CONSTRAINT [fk_LoginSessions]
    FOREIGN KEY ([SessionId])
    REFERENCES [dbo].[LoginSessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_LoginSessions'
CREATE INDEX [IX_fk_LoginSessions]
ON [dbo].[Logs]
    ([SessionId]);
GO

-- Creating foreign key on [Id] in table 'LoginSessions'
ALTER TABLE [dbo].[LoginSessions]
ADD CONSTRAINT [FK_LoginSessions_LoginSessions]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[LoginSessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'LoginSessions'
ALTER TABLE [dbo].[LoginSessions]
ADD CONSTRAINT [fk_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_User'
CREATE INDEX [IX_fk_User]
ON [dbo].[LoginSessions]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'Logs'
ALTER TABLE [dbo].[Logs]
ADD CONSTRAINT [fk_PerOrders]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_PerOrders'
CREATE INDEX [IX_fk_PerOrders]
ON [dbo].[Logs]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------