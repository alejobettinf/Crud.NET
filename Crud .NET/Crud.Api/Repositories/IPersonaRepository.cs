using Crud.Api.Models;

namespace Crud.Api.Repositories;

public interface IPersonaRepository
{
    Task InsertarAsync(Persona persona, CancellationToken cancellationToken);
    Task<Persona?> ObtenerPorIdentificacionAsync(string identificacion, CancellationToken cancellationToken);
    Task<IReadOnlyList<Persona>> ObtenerMujeresAsync(CancellationToken cancellationToken);
    Task<bool> ActualizarEdadAsync(string identificacion, int edad, CancellationToken cancellationToken);
    Task<bool> EliminarAsync(string identificacion, CancellationToken cancellationToken);
}
