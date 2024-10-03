using Deo.Frontend.Components;

const string DEO_BACKEND_URL_ENV_VAR_NAME = "DEO_BACKEND_URL";

var builder = WebApplication.CreateBuilder(args);

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

await app.RunAsync();
