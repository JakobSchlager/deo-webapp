using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var counter = new Counter();

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/", GetCounter);

async Task<string> GetCounter(ILogger<Program> logger)
{
  logger.LogInformation("Retrieving counter value");
  var value = await Task.Run(counter.Get);
  return JsonSerializer.Serialize(new { value });
}

app.MapPost("/increment", IncrementCounter);

async Task<string> IncrementCounter(ILogger<Program> logger)
{
  logger.LogInformation("Incrementing counter value");
  var (before, after) = await Task.Run(() => counter.Increment(1));
  return JsonSerializer.Serialize(new { before, after });
}

app.MapPost("/reset", ResetCounter);

async Task<string> ResetCounter(ILogger<Program> logger)
{
  logger.LogInformation("Resetting counter value");
  var (before, after) = await Task.Run(counter.Reset);
  return JsonSerializer.Serialize(new { before, after });
}

await app.RunAsync();
