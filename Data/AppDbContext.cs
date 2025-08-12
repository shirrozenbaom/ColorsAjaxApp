using Microsoft.EntityFrameworkCore;
using ColorsAjaxApp.Models;

namespace ColorsAjaxApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ColorItem> Colors => Set<ColorItem>();
}
