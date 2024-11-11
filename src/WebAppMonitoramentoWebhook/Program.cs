using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

object? lastEvent = null;

app.MapGet("/monitor", () =>
{
    app.Logger.LogInformation("GET /monitor | Último evento recebido: " +
        JsonSerializer.Serialize(lastEvent,
            options: new() { WriteIndented = true }));
    return lastEvent;
})
.WithOpenApi();

app.MapPost("/monitor", (object data) =>
{
    lastEvent = data;
    app.Logger.LogInformation("POST /monitor | Notificação recebida: " +
        JsonSerializer.Serialize(data,
            options: new() { WriteIndented = true }));
    return Results.Ok();
})
.WithOpenApi();

app.Run();