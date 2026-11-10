var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<Crud.Api.Repositories.IPersonaRepository, Crud.Api.Repositories.PersonaRepository>();
builder.Services.AddScoped<Crud.Api.Services.IPersonaService, Crud.Api.Services.PersonaService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
