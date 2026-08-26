using DuckPond;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<TipCatalogue>();
builder.Services.AddSingleton<ConsultationStore>();

var app = builder.Build();

// Resolve both singletons up front, so their startup logging shows up in
// `docker compose logs duck-api` immediately instead of whenever the first
// request happens to arrive.
var tips = app.Services.GetRequiredService<TipCatalogue>();
var consultations = app.Services.GetRequiredService<ConsultationStore>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("The duck pond is open for business. {Count} ducks on staff.",
    DuckRoster.Ducks.Count);

app.MapGet("/api/health", () => Results.Ok(new
{
    status = "healthy",
    pond = Environment.MachineName
}));

app.MapGet("/api/ducks", () => Results.Ok(DuckRoster.Ducks));

app.MapGet("/api/consultations", () => Results.Ok(consultations.ReadAll()));

app.MapPost("/api/consultations", (ConsultationRequest request) =>
{
    var duck = DuckRoster.Find(request.DuckId);
    if (duck is null)
    {
        logger.LogWarning("Somebody asked for duck {DuckId}, who does not work here.", request.DuckId);
        return Results.NotFound(new { message = $"No duck with id {request.DuckId} works here." });
    }

    var problem = request.Problem?.Trim();
    if (string.IsNullOrEmpty(problem))
    {
        return Results.BadRequest(new { message = "Describe your problem first. That is the whole point." });
    }

    var consultation = new Consultation(
        Guid.NewGuid().ToString("n"),
        duck.Id,
        duck.Name,
        duck.Emoji,
        problem,
        tips.TipFor(duck),
        DateTimeOffset.UtcNow);

    consultations.Append(consultation);

    logger.LogInformation("{Duck} was consulted about {Length} characters of problem.",
        duck.Name, problem.Length);

    return Results.Ok(consultation);
});

app.Run();
