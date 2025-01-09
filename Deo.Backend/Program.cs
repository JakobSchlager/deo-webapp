using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.Json;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

var counter = new Counter();

var deoBackendMeter = new Meter("Deo.Backend", "1.0.0");
var counterRetrievals = deoBackendMeter.CreateCounter<int>("counter.retrievals", description: "Counts the number of counter retrievals");
var counterIncrements = deoBackendMeter.CreateCounter<int>("counter.increments", description: "Counts the number of counter increments");
var counterResets = deoBackendMeter.CreateCounter<int>("counter.resets", description: "Counts the number of counter resets");

var backendActivitySource = new ActivitySource("Deo.Backend");

var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
var otel = builder.Services.AddOpenTelemetry();

otel.ConfigureResource(resource => resource
    .AddService(serviceName: builder.Environment.ApplicationName));

otel.WithMetrics(metrics => metrics
    .AddAspNetCoreInstrumentation()
    .AddMeter(deoBackendMeter.Name)
    .AddMeter("Microsoft.AspNetCore.Hosting")
    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
    .AddPrometheusExporter());

otel.WithTracing(tracing =>
{
  tracing.AddAspNetCoreInstrumentation();
  tracing.AddHttpClientInstrumentation();
  tracing.AddSource(backendActivitySource.Name);

  if (tracingOtlpEndpoint != null)
  {
    tracing.AddOtlpExporter(otlpOptions =>
     {
       otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
     });
  }
  else
  {
    tracing.AddConsoleExporter();
  }
});

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/", GetCounter);

async Task<string> GetCounter(ILogger<Program> logger)
{
  using var activity = backendActivitySource.StartActivity("CounterActivity");

  logger.LogInformation("Retrieving counter value");
  activity?.AddEvent(new ActivityEvent("Retrieving counter value"));

  var value = await Task.Run(counter.Get);
  counterRetrievals.Add(1);

  activity?.SetTag("counter.value", value);

  return JsonSerializer.Serialize(new { value });
}

app.MapPost("/increment", IncrementCounter);

async Task<string> IncrementCounter(ILogger<Program> logger)
{
  using var activity = backendActivitySource.StartActivity("CounterActivity");

  logger.LogInformation("Incrementing counter value");
  activity?.AddEvent(new ActivityEvent("Incrementing counter value"));

  var (before, after) = await Task.Run(() => counter.Increment(1));
  counterIncrements.Add(after - before);

  activity?.SetTag("counter.value.before", before);
  activity?.SetTag("counter.value.after", after);

  return JsonSerializer.Serialize(new { before, after });
}

app.MapPost("/reset", ResetCounter);

async Task<string> ResetCounter(ILogger<Program> logger)
{
  using var activity = backendActivitySource.StartActivity("CounterActivity");

  logger.LogInformation("Resetting counter value");
  activity?.AddEvent(new ActivityEvent("Resetting counter value"));

  var (before, after) = await Task.Run(counter.Reset);
  counterResets.Add(1);

  return JsonSerializer.Serialize(new { before, after });
}

app.MapPrometheusScrapingEndpoint();

await app.RunAsync();
