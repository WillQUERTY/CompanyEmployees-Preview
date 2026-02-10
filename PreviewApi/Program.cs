using PreviewApi;
using PreviewApi.Application;
using PreviewApi.Infrastructure;
using PreviewApi.Infrastructure.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ═══════════════════════════════════════════════════════════════════════
// 📦 Servicios
// ═══════════════════════════════════════════════════════════════════════

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CompanyEmployees Preview API",
        Version = "v1",
        Description = "API mínimal que demuestra el patrón CQRS",
        Contact = new OpenApiContact { Name = "Your Company" }
    });

    // Documentar endpoints
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "PreviewApi.xml");
    if (File.Exists(xmlFile))
        c.IncludeXmlComments(xmlFile);
});

// Servicios de aplicación (CQRS)
builder.Services.AddApplicationServices();

// Servicios de infraestructura (DB)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructureServices(connectionString);

var app = builder.Build();

// ═══════════════════════════════════════════════════════════════════════
// 🗄️ Base de datos
// ═══════════════════════════════════════════════════════════════════════

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    app.Logger.LogInformation("Database initialized");
}

// ═══════════════════════════════════════════════════════════════════════
// 🔧 Middleware
// ═══════════════════════════════════════════════════════════════════════

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Logger.LogInformation("🚀 CompanyEmployees Preview API started");
await app.RunAsync();
