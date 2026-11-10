namespace Crud.Api.Models;

public sealed class Persona
{
    public string Identificacion { get; init; } = string.Empty;
    public string Nombres { get; init; } = string.Empty;
    public string Apellidos { get; init; } = string.Empty;
    public int Edad { get; init; }
    public string Genero { get; init; } = string.Empty;
}
