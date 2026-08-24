using Hirepath.Api.Endpoints;
using Hirepath.Infrastructure;
using Hirepath.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Services -------------------------------------------------------------
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();

const string DevCorsPolicy = "hirepath-dev";
var allowedOrigins = builder.Configuration.GetSection("Hirepath:Cors:AllowedOrigins")
    .Get<string[]>() ?? ["http://localhost:4200"];
builder.Services.AddCors(options =>
    options.AddPolicy(DevCorsPolicy, policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

// --- Database -------------------------------------------------------------
// Apply migrations at startup so a clean machine gets a working database.
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HirepathDbContext>();
    await db.Database.MigrateAsync();
}

// --- Pipeline -------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(DevCorsPolicy);

app.MapMetaEndpoints();

app.Run();

/// <summary>Exposed so the integration test project can drive the app.</summary>
public partial class Program;
