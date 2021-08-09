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
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210809215207_Estrategia03')
BEGIN
    CREATE TABLE [Pessoas] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(max) NULL,
        [TenantId] nvarchar(max) NULL,
        CONSTRAINT [PK_Pessoas] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210809215207_Estrategia03')
BEGIN
    CREATE TABLE [Produtos] (
        [Id] int NOT NULL IDENTITY,
        [Descricao] nvarchar(max) NULL,
        [TenantId] nvarchar(max) NULL,
        CONSTRAINT [PK_Produtos] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210809215207_Estrategia03')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome', N'TenantId') AND [object_id] = OBJECT_ID(N'[Pessoas]'))
        SET IDENTITY_INSERT [Pessoas] ON;
    EXEC(N'INSERT INTO [Pessoas] ([Id], [Nome], [TenantId])
    VALUES (1, N''Pessoa 1'', N''tenant-1''),
    (2, N''Pessoa 2'', N''tenant-2''),
    (3, N''Pessoa 3'', N''tenant-2'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome', N'TenantId') AND [object_id] = OBJECT_ID(N'[Pessoas]'))
        SET IDENTITY_INSERT [Pessoas] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210809215207_Estrategia03')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Descricao', N'TenantId') AND [object_id] = OBJECT_ID(N'[Produtos]'))
        SET IDENTITY_INSERT [Produtos] ON;
    EXEC(N'INSERT INTO [Produtos] ([Id], [Descricao], [TenantId])
    VALUES (1, N''Descricao 1'', N''tenant-1''),
    (2, N''Descricao 2'', N''tenant-2''),
    (3, N''Descricao 3'', N''tenant-2'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Descricao', N'TenantId') AND [object_id] = OBJECT_ID(N'[Produtos]'))
        SET IDENTITY_INSERT [Produtos] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210809215207_Estrategia03')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210809215207_Estrategia03', N'5.0.8');
END;
GO

COMMIT;
GO

