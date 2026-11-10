IF DB_ID(N'EpikCrud') IS NULL
BEGIN
    CREATE DATABASE EpikCrud;
END;
GO

USE EpikCrud;
GO

IF OBJECT_ID(N'dbo.Persona', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Persona
    (
        Identificacion NVARCHAR(30) NOT NULL,
        Nombres        NVARCHAR(100) NOT NULL,
        Apellidos      NVARCHAR(100) NOT NULL,
        Edad           TINYINT NOT NULL,
        Genero         NVARCHAR(10) NOT NULL,
        CONSTRAINT PK_Persona PRIMARY KEY (Identificacion),
        CONSTRAINT CK_Persona_Edad CHECK (Edad BETWEEN 0 AND 120),
        CONSTRAINT CK_Persona_Genero CHECK (Genero IN (N'Masculino', N'Femenino'))
    );
END;
GO
