using CopilotInstructions.Web.Components;
using CopilotInstructions.Web.Models;
using CopilotInstructions.Web.Services;
using MudBlazor.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configure the application to use configuration from various sources
// Order matters: later configurations override earlier ones
if (builder.Environment.IsDevelopment())
{
    // Explicitly add user secrets when in development
    builder.Configuration.AddUserSecrets<Program>();
    Console.WriteLine("User secrets have been loaded for development environment.");
}

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure GitHub settings
builder.Services.Configure<GitHubConfig>(
    builder.Configuration.GetSection("GitHub"));

builder.Services.AddScoped<ICopilotInstructionsService, CopilotInstructionsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Log the configuration source providers in development mode
    foreach (var provider in ((IConfigurationRoot)app.Configuration).Providers)
    {
        Console.WriteLine($"Configuration provider: {provider.GetType().Name}");
    }
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
