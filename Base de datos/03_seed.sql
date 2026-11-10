USE EpikCrud;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Persona WHERE Identificacion = N'1000001')
BEGIN
    INSERT INTO dbo.Persona (Identificacion, Nombres, Apellidos, Edad, Genero)
    VALUES (N'1000001', N'Ana', N'Gomez', 28, N'Femenino');
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Persona WHERE Identificacion = N'1000002')
BEGIN
    INSERT INTO dbo.Persona (Identificacion, Nombres, Apellidos, Edad, Genero)
    VALUES (N'1000002', N'Carlos', N'Perez', 34, N'Masculino');
END;
GO
