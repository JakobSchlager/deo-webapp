using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Deo.Frontend.Components;

const string DEO_BACKEND_URL_ENV_VAR_NAME = "DEO_BACKEND_URL";

var builder = WebApplication.CreateBuilder(args);

var deoFrontendMeter = new Meter("Deo.Frontend", "1.0.0");

var frontendActivitySource = new ActivitySource("Deo.Frontend");

var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
var otel = builder.Services.AddOpenTelemetry();

otel.ConfigureResource(resource => resource
    .AddService(serviceName: builder.Environment.ApplicationName));

otel.WithMetrics(metrics => metrics
    .AddAspNetCoreInstrumentation()
    .AddMeter(deoFrontendMeter.Name)
    .AddMeter("Microsoft.AspNetCore.Hosting")
    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
    .AddPrometheusExporter());

otel.WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation();
    tracing.AddHttpClientInstrumentation();
    tracing.AddSource(frontendActivitySource.Name);

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

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<CounterHttpClient>(client =>
    client.BaseAddress = new Uri(
        builder.Configuration[DEO_BACKEND_URL_ENV_VAR_NAME]
            ?? throw new InvalidOperationException($"Environment variable \"{DEO_BACKEND_URL_ENV_VAR_NAME}\" not set.")
    )
);

builder.Services.AddScoped<ICounterService, CounterService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPrometheusScrapingEndpoint();

await app.RunAsync();
