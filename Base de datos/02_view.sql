USE EpikCrud;
GO

CREATE OR ALTER VIEW dbo.vw_PersonasMujeres
AS
    SELECT Identificacion, Nombres, Apellidos, Edad, Genero
    FROM dbo.Persona
    WHERE Genero = N'Femenino';
GO
