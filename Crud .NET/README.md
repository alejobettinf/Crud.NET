# Prueba EPIK - CRUD de Persona

Construí una API REST con ASP.NET Core sobre .NET 10 y la conecté a SQL Server. La solución
permite crear, consultar, actualizar y eliminar personas, incluyendo el listado
de mujeres mediante una vista de base de datos.

## Estructura de la entrega

Organicé el ZIP siguiendo la estructura solicitada en la prueba:

- `Base de datos`: scripts, vista, datos de ejemplo y backup de `EpikCrud`.
- `Crud .NET`: solución, API, pruebas unitarias, colección de Postman y documentación.

Para ejecutar la solución, abrir una terminal dentro de la carpeta `Crud .NET`.

Elegí separar la solución en Controller, Service y Repository porque esta
estructura me permite mantener separada la entrada HTTP, las reglas de la
aplicación y las consultas SQL.

## La entrega incluye

- `Crud.Api`: API REST en ASP.NET Core.
- `Crud.Api.Tests`: pruebas unitarias de los controllers.
- `Base de datos`: scripts para crear la base, la tabla, la vista y datos iniciales.
- `Base de datos/EpikCrud.bak`: backup validado de la base de datos.
- `Pruebas/Epik.postman_collection.json`: colección para validar los endpoints.
- `docker-compose.yml`: configuración para ejecutar SQL Server localmente.

## Requisitos

- .NET 10 SDK.
- Docker Desktop con WSL 2.
- SQL Server Management Studio, opcional para ejecutar los scripts.

## 1. Levantar SQL Server

Desde la raíz del proyecto:

```powershell
Copy-Item .env.example .env
docker compose up -d
```

El archivo `.env` define la variable `MSSQL_SA_PASSWORD`. Docker Compose la
lee automáticamente porque está en la misma carpeta que `docker-compose.yml`.
La instancia queda disponible en `localhost,1433`.

Desde SQL Server Management Studio, ejecutar los scripts en este orden:

1. `Base de datos/01_schema.sql`
2. `Base de datos/02_view.sql`
3. `Base de datos/03_seed.sql`

Con esto quedan creadas la base `EpikCrud`, la tabla `Persona`, la vista
`vw_PersonasMujeres` y dos registros iniciales para facilitar la validación.

## 2. Configurar la conexión de la API

Uso .NET User Secrets para no guardar la cadena de conexión dentro del código.
Para facilitar la revisión técnica, dejo el comando completo:

```powershell
dotnet user-secrets set "ConnectionStrings:EpikCrud" "Server=localhost,1433;Database=EpikCrud;User Id=sa;Password=EpikSql_2026!Dev42;TrustServerCertificate=True;" --project Crud.Api/Crud.Api.csproj
```

La contraseña se incluye de forma explícita en el README y en `.env.example`
para que el evaluador pueda ejecutar la prueba sin tener que descubrir o
reconstruir credenciales.

## 3. Ejecutar la API

```powershell
$apiUrl = 'http' + '://localhost:5080'
dotnet run --project Crud.Api/Crud.Api.csproj --urls $apiUrl
```

Para comprobar que la API inició correctamente:

```powershell
$apiUrl = 'http' + '://localhost:5080'
Invoke-RestMethod "$apiUrl/api/health"
```

Debe responder:

```json
{
  "status": "ok",
  "service": "Crud.Api"
}
```

El documento OpenAPI queda disponible en la ruta `/openapi/v1.json`:

`http://localhost:5080/openapi/v1.json`

## 4. Endpoints solicitados

| Método | Ruta | Resultado esperado |
|---|---|---|
| POST | `/api/personas` | Inserta una persona y responde `201 Created` |
| GET | `/api/personas/{identificacion}` | Consulta una persona por identificación |
| GET | `/api/personas/mujeres` | Lista mujeres consultando `vw_PersonasMujeres` |
| PATCH | `/api/personas/{identificacion}/edad` | Actualiza únicamente la edad |
| DELETE | `/api/personas/{identificacion}` | Elimina por identificación |

La colección de Postman está en `Pruebas/Epik.postman_collection.json`.

## 5. Ejecutar las pruebas unitarias

Desde la raíz del proyecto:

```powershell
dotnet test EpikCrud.slnx
```

Las pruebas unitarias validan respuestas importantes de la API sin depender de
la base de datos. También realicé pruebas manuales contra SQL Server para
comprobar el flujo completo de insertar, consultar, listar mujeres, actualizar
y eliminar.

## Nota sobre dependencias

Al restaurar los paquetes, NuGet puede mostrar una advertencia sobre una
dependencia transitiva utilizada por `Microsoft.Data.SqlClient`. Revisé las
versiones disponibles y no hay una actualización del paquete principal en los
orígenes configurados. La solución compila y las pruebas pasan correctamente;
mantengo la advertencia visible para tener claridad sobre ella.

## 6. Detener SQL Server

```powershell
docker compose stop
```

El volumen de Docker conserva los datos. No ejecutar `docker compose down -v`
durante la revisión, porque ese comando elimina el volumen y la base local.
