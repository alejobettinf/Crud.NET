using System.ComponentModel.DataAnnotations;

namespace Crud.Api.Contracts;

public sealed class ActualizarEdadRequest
{
    [Range(0, 120)]
    public int Edad { get; init; }
}
