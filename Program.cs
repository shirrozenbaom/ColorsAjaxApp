using Microsoft.EntityFrameworkCore;
using ColorsAjaxApp.Data;
using ColorsAjaxApp.Models;

var builder = WebApplication.CreateBuilder(args);

// EF + SQLite
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers(); // ל-API
builder.Services.AddRazorPages();  // לדף הראשי

var app = builder.Build();

app.UseStaticFiles();
app.MapRazorPages();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.EnsureCreated() && !db.Colors.Any())
    {
        db.Colors.AddRange(
            new ColorItem { Name = "ורוד", Price = 35, DisplayOrder = 1, InStock = true },
            new ColorItem { Name = "צהוב", Price = 42, DisplayOrder = 2, InStock = true },
            new ColorItem { Name = "כחול", Price = 28, DisplayOrder = 3, InStock = false }
        );
        db.SaveChanges();
    }
}

app.Run();
