using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OneUron.BLL.DTOs.Settings;
using OneUron.BLL.Interface;
using OneUron.BLL.Services; // Add this using directive for AuthService
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository;
using OneUron.DAL.Repository.AcknowledgeRepo;
using OneUron.DAL.Repository.CourseDetailRepo;
using OneUron.DAL.Repository.EnRollRepo;
using OneUron.DAL.Repository.IntructorRepo;
using OneUron.DAL.Repository.ResourceRepo;
using OneUron.DAL.Repository.SkillRepo;
using OneUron.DAL.Repository.TokenRepo;
using OneUron.DAL.Repository.UserRepo; // Ensure this using directive is present


var builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = builder.Configuration;
builder.Services.Configure<JwtSettings>(configuration.GetSection("AppSettings"));

// Register configuration as singleton for DI
builder.Services.AddSingleton<IConfiguration>(configuration);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OneUron API", Version = "v1" });
});


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IResourcesRepository, ResourcesRepository>();
builder.Services.AddScoped<IResourcesService, ResourcesService>();
builder.Services.AddScoped<IEnRollRepository, EnRollRepository>();
builder.Services.AddScoped<IEnRollService, EnRollService>();
builder.Services.AddScoped<ICourseDetailRepository, CourseDetailRepository>();
builder.Services.AddScoped<ICourseDetailService, CourseDetailService>();
builder.Services.AddScoped<IAcknowledgeRepository, AcknowledgeRepository>();
builder.Services.AddScoped<IAcknowledgeService, AcknowledgeService>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

// Configure CORS
app.UseCors("AllowSpecificOrigins");

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
