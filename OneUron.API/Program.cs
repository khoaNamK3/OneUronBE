using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));


var app = builder.Build();

// Middleware pipeline
app.UseAuthorization();

app.MapControllers();

app.Run();
