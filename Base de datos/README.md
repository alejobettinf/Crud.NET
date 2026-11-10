# Base de datos de la prueba EPIK

Preparé estos scripts para que el evaluador pueda crear la base de datos desde
cero y revisar directamente la tabla y la vista que utiliza la API.

Ejecutar desde SQL Server Management Studio en este orden:

1. `01_schema.sql`
2. `02_view.sql`
3. `03_seed.sql`

La base se llama `EpikCrud`. La vista `dbo.vw_PersonasMujeres` debe ser usada
por el endpoint que lista las mujeres. El tercer script agrega dos registros de
ejemplo para facilitar la validación manual.

También incluí `EpikCrud.bak`, un backup validado de la base de datos ya creada.
