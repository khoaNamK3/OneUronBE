using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using OneUron.BLL.DTOs.Settings;
using OneUron.BLL.Interface;
using OneUron.BLL.Services; // Add this using directive for AuthService
using OneUron.BLL.Services.Ai;
using OneUron.BLL.Until;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository;
using OneUron.DAL.Repository.AcknowledgeRepo;
using OneUron.DAL.Repository.AnswerRepo;
using OneUron.DAL.Repository.ChoiceRepo;
using OneUron.DAL.Repository.CourseDetailRepo;
using OneUron.DAL.Repository.EnRollRepo;
using OneUron.DAL.Repository.EvaluationQuestionRepo;
using OneUron.DAL.Repository.EvaluationRepo;
using OneUron.DAL.Repository.IntructorRepo;
using OneUron.DAL.Repository.MethodConRepo;
using OneUron.DAL.Repository.MethodProRepo;
using OneUron.DAL.Repository.MethodRepo;
using OneUron.DAL.Repository.MethodRuleConditionRepo;
using OneUron.DAL.Repository.MethodRulesRepo;
using OneUron.DAL.Repository.ProfileRepository;
using OneUron.DAL.Repository.QuestionChoiceRepo;
using OneUron.DAL.Repository.QuestionRepo;
using OneUron.DAL.Repository.QuizRepo;
using OneUron.DAL.Repository.ResourceRepo;
using OneUron.DAL.Repository.RoleRepo;
using OneUron.DAL.Repository.SkillRepo;
using OneUron.DAL.Repository.StudyMethodRepo;
using OneUron.DAL.Repository.TechniqueRepo;
using OneUron.DAL.Repository.TokenRepo;
using OneUron.DAL.Repository.UserAnswerRepo;
using OneUron.DAL.Repository.UserQuizAttemptRepo;
using OneUron.DAL.Repository.UserRepo; // Ensure this using directive is present
using System.Text;

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

// Add Authentication with JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });

// Add Authorization with policies for different roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireTeacherRole", policy => policy.RequireRole("Teacher"));
    options.AddPolicy("RequireStudentRole", policy => policy.RequireRole("Student"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OneUron API", Version = "v1" });

  
    c.MapType<TimeOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "time",
        Example = new OpenApiString("08:30:00")
    });

    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
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
builder.Services.AddScoped<IMethodRepository, MethodRepository>();
builder.Services.AddScoped<IMethodSerivce, MethodSerivce>();
builder.Services.AddScoped<IStudyMethodRepository, StudyMethodRepository>();
builder.Services.AddScoped<IStudyMethodService, StudyMethodService>();
builder.Services.AddScoped<IMethodProRepository, MethodProRepository>();
builder.Services.AddScoped<IMethodProSerivce, MethodProSerivce>();
builder.Services.AddScoped<IMethodConRepository, MethodConRepository>();
builder.Services.AddScoped<IMethodConService, MethodConService>();
builder.Services.AddScoped<ITechniqueRepository, TechniqueRepository>();
builder.Services.AddScoped<ITechniqueService, TechniqueService>();
builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();
builder.Services.AddScoped<IEvaluationService, EvaluationService>();
builder.Services.AddScoped<IEvaluationQuestionRepository, EvaluationQuestionRepository>();
builder.Services.AddScoped<IEvaluationQuestionService, EvaluationQuestionService>();
builder.Services.AddScoped<IChoiceRepository, ChoiceRepository>();
builder.Services.AddScoped<IChoiceService, ChoiceService>();
builder.Services.AddScoped<IUserAnswerRepository, UserAnswerRepository>();
builder.Services.AddScoped<IUserAnswerService, UserAnswerService>();
builder.Services.AddScoped<IMethodRuleConditionRepository, MethodRuleConditionRepository>();
builder.Services.AddScoped<IMethodRuleConditionService, MethodRuleConditionService>();
builder.Services.AddScoped<IMethodRuleRepository, MethodRuleRepository>();
builder.Services.AddScoped<IMethodRuleService, MethodRuleService>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IUserQuizAttemptReposiotry, UserQuizAttemptReposiotry>();
builder.Services.AddScoped<IUserQuizAttemptService, UserQuizAttemptService>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionChoiceRepository, QuestionChoiceRepository>();
builder.Services.AddScoped<IQuestionChoiceService, QuestionChoiceService>();
builder.Services.AddScoped<IGeminiService, GeminiService>();
// Register repositories and services
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();

// Add database initialization service
builder.Services.AddSingleton<DbInitializer>();
builder.Services.AddSingleton<OneUron.BLL.Services.IDbInitializer>(sp => sp.GetRequiredService<DbInitializer>());
builder.Services.AddHostedService(sp => sp.GetRequiredService<DbInitializer>());

var app = builder.Build();

// Configure CORS
app.UseCors("AllowSpecificOrigins");

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
