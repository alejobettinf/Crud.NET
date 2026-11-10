using System.ComponentModel.DataAnnotations;

namespace Crud.Api.Contracts;

public sealed class CrearPersonaRequest
{
    [Required, StringLength(30)]
    public string Identificacion { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Nombres { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string Apellidos { get; init; } = string.Empty;

    [Range(0, 120)]
    public int Edad { get; init; }

    [Required, RegularExpression("^(Masculino|Femenino)$")]
    public string Genero { get; init; } = string.Empty;
}
