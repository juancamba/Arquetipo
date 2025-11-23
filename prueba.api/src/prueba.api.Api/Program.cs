
//using Cortex.Mediator.DependencyInjection;

using Mediator;
using prueba.api.Api.dto;
using prueba.api.Application;
using prueba.api.Application.Alquiler.CrearAlquiler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddFeatures(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapPost("/alquiler", async (CrearAlquilerDto request, IMediator mediator ) =>
{
    var command = new CrearAlquilerCommand(request.IdCliente, request.IdVehiculo);
    var result = await mediator.Send(command, CancellationToken.None);
    
    return Results.Ok(result);   
    
    //return Results.Ok();
}).WithName("alquiler")
.WithOpenApi();

app.Run();

