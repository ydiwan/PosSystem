using Microsoft.EntityFrameworkCore;
using Pos.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// EF Core + SQLite
builder.Services.AddDbContext<PosDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Create database & tables if they don't exist
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PosDbContext>();
    db.Database.EnsureCreated();   // 👈 instead of Migrate()
    DbSeeder.Seed(db);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

