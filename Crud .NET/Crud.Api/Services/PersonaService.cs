using Crud.Api.Models;
using Crud.Api.Repositories;

namespace Crud.Api.Services;

public sealed class PersonaService(IPersonaRepository repository) : IPersonaService
{
    public Task InsertarAsync(Persona persona, CancellationToken cancellationToken) =>
        repository.InsertarAsync(persona, cancellationToken);

    public Task<Persona?> ObtenerPorIdentificacionAsync(string identificacion, CancellationToken cancellationToken) =>
        repository.ObtenerPorIdentificacionAsync(identificacion, cancellationToken);

    public Task<IReadOnlyList<Persona>> ObtenerMujeresAsync(CancellationToken cancellationToken) =>
        repository.ObtenerMujeresAsync(cancellationToken);

    public Task<bool> ActualizarEdadAsync(string identificacion, int edad, CancellationToken cancellationToken) =>
        repository.ActualizarEdadAsync(identificacion, edad, cancellationToken);

    public Task<bool> EliminarAsync(string identificacion, CancellationToken cancellationToken) =>
        repository.EliminarAsync(identificacion, cancellationToken);
}
