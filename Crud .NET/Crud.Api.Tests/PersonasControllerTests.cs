using Crud.Api.Contracts;
using Crud.Api.Controllers;
using Crud.Api.Models;
using Crud.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Api.Tests;

public sealed class PersonasControllerTests
{
    [Fact]
    public async Task ObtenerMujeres_DebeDevolverLaListaDelServicio()
    {
        var mujeres = new List<Persona>
        {
            new()
            {
                Identificacion = "1000001",
                Nombres = "Ana",
                Apellidos = "Gomez",
                Edad = 28,
                Genero = "Femenino"
            }
        };
        var servicio = new FakePersonaService { Mujeres = mujeres };
        var controller = new PersonasController(servicio);

        var result = await controller.ObtenerMujeres(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(mujeres, ok.Value);
    }

    [Fact]
    public async Task ObtenerPorIdentificacion_CuandoNoExiste_DebeDevolver404()
    {
        var controller = new PersonasController(new FakePersonaService());

        var result = await controller.ObtenerPorIdentificacion("9999999", CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Insertar_DebeMapearLaSolicitudYDevolver201()
    {
        var servicio = new FakePersonaService();
        var controller = new PersonasController(servicio);
        var request = new CrearPersonaRequest
        {
            Identificacion = "9000001",
            Nombres = "Laura",
            Apellidos = "Prueba",
            Edad = 31,
            Genero = "Femenino"
        };

        var result = await controller.Insertar(request, CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var persona = Assert.IsType<Persona>(created.Value);
        Assert.Equal("9000001", persona.Identificacion);
        Assert.Equal(31, persona.Edad);
        Assert.Equal("Femenino", persona.Genero);
        Assert.NotNull(servicio.PersonaInsertada);
    }

    [Fact]
    public async Task ActualizarEdad_CuandoNoExiste_DebeDevolver404()
    {
        var controller = new PersonasController(new FakePersonaService { ActualizacionExitosa = false });

        var result = await controller.ActualizarEdad(
            "9999999",
            new ActualizarEdadRequest { Edad = 40 },
            CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Eliminar_CuandoExiste_DebeDevolver204()
    {
        var controller = new PersonasController(new FakePersonaService { EliminacionExitosa = true });

        var result = await controller.Eliminar("9000001", CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    private sealed class FakePersonaService : IPersonaService
    {
        public IReadOnlyList<Persona> Mujeres { get; init; } = [];
        public Persona? PersonaInsertada { get; private set; }
        public bool ActualizacionExitosa { get; init; } = true;
        public bool EliminacionExitosa { get; init; }

        public Task InsertarAsync(Persona persona, CancellationToken cancellationToken)
        {
            PersonaInsertada = persona;
            return Task.CompletedTask;
        }

        public Task<Persona?> ObtenerPorIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken) =>
            Task.FromResult<Persona?>(null);

        public Task<IReadOnlyList<Persona>> ObtenerMujeresAsync(CancellationToken cancellationToken) =>
            Task.FromResult(Mujeres);

        public Task<bool> ActualizarEdadAsync(
            string identificacion,
            int edad,
            CancellationToken cancellationToken) =>
            Task.FromResult(ActualizacionExitosa);

        public Task<bool> EliminarAsync(string identificacion, CancellationToken cancellationToken) =>
            Task.FromResult(EliminacionExitosa);
    }
}
