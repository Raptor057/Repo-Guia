-------------------------------------
-- 1. Crear LOGIN a nivel servidor
-------------------------------------
USE [master];
GO

CREATE LOGIN [AppUser] WITH PASSWORD = 'TuC0ntraseña_Fuert3!',
    CHECK_POLICY = ON,         -- aplica política de seguridad de Windows (longitud, complejidad, etc.)
    CHECK_EXPIRATION = OFF;    -- si quieres que no caduque la contraseña
GO

-------------------------------------
-- 2. Crear la base de datos
-------------------------------------
IF DB_ID('Project') IS NULL
BEGIN
    CREATE DATABASE [Project];
END
GO

USE [Project];
GO

-------------------------------------
-- 3. Crear USER en la BD para el LOGIN
-------------------------------------
CREATE USER [AppUser] FOR LOGIN [AppUser];
GO

-- Opción A: darle control total sobre la BD (db_owner)
ALTER ROLE [db_owner] ADD MEMBER [AppUser];
GO
-- Opción B (alternativa): dar solo lectura/escritura
-- ALTER ROLE [db_datareader] ADD MEMBER [AppUser];
-- ALTER ROLE [db_datawriter] ADD MEMBER [AppUser];

-------------------------------------
-- 4. Crear tablas y datos de catálogo
-------------------------------------
IF OBJECT_ID('dbo.SystemRoles', 'U') IS NULL
BEGIN
    CREATE TABLE [SystemRoles](
        [RolID] BIGINT IDENTITY(1,1) PRIMARY KEY,
        [Rol]   NVARCHAR(50) NOT NULL
    );
END
GO

IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE [Users](
        [UserID]       BIGINT IDENTITY(1,1) PRIMARY KEY,
        [UserFullName] NVARCHAR(100) NOT NULL,
        [UserName]     NVARCHAR(50)  NOT NULL,
        [PasswordHash] VARBINARY(256) NOT NULL,
        [UserRolID]    BIGINT NOT NULL,
        [UtcTimeStamp] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
        [UserActive]   BIT NOT NULL DEFAULT (1),
        CONSTRAINT FK_Users_SystemRoles
            FOREIGN KEY (UserRolID) REFERENCES [SystemRoles](RolID)
    );

    CREATE UNIQUE INDEX UX_Users_UserName ON [Users]([UserName]);
END
GO

-- Semilla de roles si no existen
IF NOT EXISTS (SELECT 1 FROM [SystemRoles])
BEGIN
    INSERT INTO [SystemRoles] (Rol) VALUES ('Admin'),('User');
END
GO
