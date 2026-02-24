using Arquetipo.Api;
using Arquetipo.Api.Configuration;
using Arquetipo.Api.Extensions;
using Arquetipo.Api.Middleware;
using Arquetipo.Application;
using Arquetipo.Infrastructure;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();



builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApi(builder.Configuration);

var app = builder.Build();
Console.WriteLine($"DOTNET ENV: {builder.Environment.EnvironmentName}");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    await app.ApplyMigration();
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();


app.UseApi();

app.Run();


public partial class Program;