using Crud.Api.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Crud.Api.Repositories;

public sealed class PersonaRepository(IConfiguration configuration) : IPersonaRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("EpikCrud")
        ?? throw new InvalidOperationException("No se encontro la cadena de conexion EpikCrud.");

    public async Task InsertarAsync(Persona persona, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO dbo.Persona (Identificacion, Nombres, Apellidos, Edad, Genero)
            VALUES (@Identificacion, @Nombres, @Apellidos, @Edad, @Genero);
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AgregarParametros(command, persona);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<Persona?> ObtenerPorIdentificacionAsync(string identificacion, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT Identificacion, Nombres, Apellidos, Edad, Genero
            FROM dbo.Persona
            WHERE Identificacion = @Identificacion;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Identificacion", SqlDbType.NVarChar, 30).Value = identificacion;

        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? Mapear(reader) : null;
    }

    public async Task<IReadOnlyList<Persona>> ObtenerMujeresAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT Identificacion, Nombres, Apellidos, Edad, Genero
            FROM dbo.vw_PersonasMujeres
            ORDER BY Apellidos, Nombres;
            """;

        var personas = new List<Persona>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            personas.Add(Mapear(reader));
        }

        return personas;
    }

    public async Task<bool> ActualizarEdadAsync(string identificacion, int edad, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE dbo.Persona
            SET Edad = @Edad
            WHERE Identificacion = @Identificacion;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Edad", SqlDbType.TinyInt).Value = edad;
        command.Parameters.Add("@Identificacion", SqlDbType.NVarChar, 30).Value = identificacion;
        return await command.ExecuteNonQueryAsync(cancellationToken) == 1;
    }

    public async Task<bool> EliminarAsync(string identificacion, CancellationToken cancellationToken)
    {
        const string sql = "DELETE FROM dbo.Persona WHERE Identificacion = @Identificacion;";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Identificacion", SqlDbType.NVarChar, 30).Value = identificacion;
        return await command.ExecuteNonQueryAsync(cancellationToken) == 1;
    }

    private static void AgregarParametros(SqlCommand command, Persona persona)
    {
        command.Parameters.Add("@Identificacion", SqlDbType.NVarChar, 30).Value = persona.Identificacion;
        command.Parameters.Add("@Nombres", SqlDbType.NVarChar, 100).Value = persona.Nombres;
        command.Parameters.Add("@Apellidos", SqlDbType.NVarChar, 100).Value = persona.Apellidos;
        command.Parameters.Add("@Edad", SqlDbType.TinyInt).Value = persona.Edad;
        command.Parameters.Add("@Genero", SqlDbType.NVarChar, 10).Value = persona.Genero;
    }

    private static Persona Mapear(SqlDataReader reader) => new()
    {
        Identificacion = reader.GetString(0),
        Nombres = reader.GetString(1),
        Apellidos = reader.GetString(2),
        Edad = reader.GetByte(3),
        Genero = reader.GetString(4)
    };
}
