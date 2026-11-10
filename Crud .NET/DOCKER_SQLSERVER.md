# SQL Server local para la prueba EPIK

La base de datos se ejecutara en Docker. La API .NET se ejecutara directamente
desde Windows.

## Conexion

- Servidor: `localhost,1433`
- Usuario: `sa`
- Base de datos: la crearemos durante la implementacion
- Contrasena: la definida en el archivo local `.env`

## Comandos

Crear `.env` a partir de `.env.example` y luego:

```powershell
Copy-Item .env.example .env
docker compose up -d
docker compose ps
docker compose logs -f sqlserver
```

El archivo `.env.example` ya contiene la contraseña local de demostración para
facilitar la validación del evaluador. El archivo `.env` no se debe compartir.

Para detener SQL Server sin eliminar sus datos:

```powershell
docker compose stop
```

Para volver a iniciarlo:

```powershell
docker compose start
```

El volumen `epik-sqlserver-data` conserva los datos. No se debe ejecutar
`docker compose down -v` durante el desarrollo, porque elimina el volumen de la
base de datos.
