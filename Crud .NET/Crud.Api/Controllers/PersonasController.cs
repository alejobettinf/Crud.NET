using Crud.Api.Contracts;
using Crud.Api.Models;
using Crud.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Crud.Api.Controllers;

[ApiController]
[Route("api/personas")]
public sealed class PersonasController(IPersonaService service) : ControllerBase
{
    [HttpGet("mujeres")]
    public async Task<ActionResult<IReadOnlyList<Persona>>> ObtenerMujeres(CancellationToken cancellationToken)
    {
        return Ok(await service.ObtenerMujeresAsync(cancellationToken));
    }

    [HttpGet("{identificacion}")]
    public async Task<ActionResult<Persona>> ObtenerPorIdentificacion(
        string identificacion,
        CancellationToken cancellationToken)
    {
        var persona = await service.ObtenerPorIdentificacionAsync(identificacion, cancellationToken);
        return persona is null ? NotFound() : Ok(persona);
    }

    [HttpPost]
    public async Task<ActionResult<Persona>> Insertar(
        CrearPersonaRequest request,
        CancellationToken cancellationToken)
    {
        var persona = new Persona
        {
            Identificacion = request.Identificacion,
            Nombres = request.Nombres,
            Apellidos = request.Apellidos,
            Edad = request.Edad,
            Genero = request.Genero
        };

        try
        {
            await service.InsertarAsync(persona, cancellationToken);
        }
        catch (SqlException exception) when (exception.Number is 2601 or 2627)
        {
            return Conflict(new { message = "Ya existe una persona con esa identificacion." });
        }

        return CreatedAtAction(nameof(ObtenerPorIdentificacion),
            new { identificacion = persona.Identificacion }, persona);
    }

    [HttpPatch("{identificacion}/edad")]
    public async Task<IActionResult> ActualizarEdad(
        string identificacion,
        ActualizarEdadRequest request,
        CancellationToken cancellationToken)
    {
        var actualizado = await service.ActualizarEdadAsync(identificacion, request.Edad, cancellationToken);
        return actualizado ? NoContent() : NotFound();
    }

    [HttpDelete("{identificacion}")]
    public async Task<IActionResult> Eliminar(string identificacion, CancellationToken cancellationToken)
    {
        var eliminado = await service.EliminarAsync(identificacion, cancellationToken);
        return eliminado ? NoContent() : NotFound();
    }
}
