using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Net.payOS;
using OneUron.BLL.DTOs.AcknowledgeDTOs;
using OneUron.BLL.DTOs.AnswerDTOs;
using OneUron.BLL.DTOs.ChoiceDTOs;
using OneUron.BLL.DTOs.CourseDetailDTOs;
using OneUron.BLL.DTOs.EnRollDTOs;
using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using OneUron.BLL.DTOs.FeatureDTOs;
using OneUron.BLL.DTOs.InstructorDTOs;
using OneUron.BLL.DTOs.MemberShipDTOs;
using OneUron.BLL.DTOs.MemberShipPlanDTOs;
using OneUron.BLL.DTOs.MethodConDTOs;
using OneUron.BLL.DTOs.MethodDTOs;
using OneUron.BLL.DTOs.MethodProDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.DTOs.PaymentDTOs;
using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.DTOs.ResourceDTOs;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.DTOs.Settings;
using OneUron.BLL.DTOs.SkillDTOs;
using OneUron.BLL.DTOs.StudyMethodDTOs;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.DTOs.TechniqueDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.BLL.Services; 
using OneUron.BLL.Services.Ai;
using OneUron.BLL.Services.UserServices;
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
using OneUron.DAL.Repository.featureRepo;
using OneUron.DAL.Repository.IntructorRepo;
using OneUron.DAL.Repository.MemberShipPlanRepo;
using OneUron.DAL.Repository.MemberShipRepo;
using OneUron.DAL.Repository.MethodConRepo;
using OneUron.DAL.Repository.MethodProRepo;
using OneUron.DAL.Repository.MethodRepo;
using OneUron.DAL.Repository.MethodRuleConditionRepo;
using OneUron.DAL.Repository.MethodRulesRepo;
using OneUron.DAL.Repository.PaymentRepo;
using OneUron.DAL.Repository.ProcessRepo;
using OneUron.DAL.Repository.ProcessTaskRepo;
using OneUron.DAL.Repository.ProfileRepository;
using OneUron.DAL.Repository.QuestionChoiceRepo;
using OneUron.DAL.Repository.QuestionRepo;
using OneUron.DAL.Repository.QuizRepo;
using OneUron.DAL.Repository.ResourceRepo;
using OneUron.DAL.Repository.RoleRepo;
using OneUron.DAL.Repository.ScheduleRepo;
using OneUron.DAL.Repository.SkillRepo;
using OneUron.DAL.Repository.StudyMethodRepo;
using OneUron.DAL.Repository.SubjectRepo;
using OneUron.DAL.Repository.TechniqueRepo;
using OneUron.DAL.Repository.TokenRepo;
using OneUron.DAL.Repository.UserAnswerRepo;
using OneUron.DAL.Repository.UserQuizAttemptRepo;
using OneUron.DAL.Repository.UserRepo; 
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


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

builder.Services.Configure<PayOsSettings>(
    builder.Configuration.GetSection("PayOs"));

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var clientId = config["PayOs:ClientId"];
    var apiKey = config["PayOs:ApiKey"];
    var checksum = config["PayOs:CheckSum"];
    return new PayOS(clientId, apiKey, checksum);
});

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

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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
builder.Services.AddScoped<IMethodSerivce, MethodService>();
builder.Services.AddScoped<IStudyMethodRepository, StudyMethodRepository>();
builder.Services.AddScoped<IStudyMethodService, StudyMethodService>();
builder.Services.AddScoped<IMethodProRepository, MethodProRepository>();
builder.Services.AddScoped<IMethodProSerivce, MethodProService>();
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
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IProcessRepository, ProcessRepository>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IProcessTaskRepository, ProcessTaskRepository>();
builder.Services.AddScoped<IProcessTaskService, ProcessTaskService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IMemberShipPlanRepository, MemberShipPlanRepository>();
builder.Services.AddScoped<IMemberShipPlanService, MemberShipPlanService>();
builder.Services.AddScoped<IMemberShipRepository, MemberShipRepository>();
builder.Services.AddScoped<IMemberShipService, MemberShipService>();
builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
builder.Services.AddScoped<IfeatureService, featureService>();
builder.Services.AddScoped<PayOsService>();
// Register repositories and services
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();

//Add validation for request 
builder.Services.AddScoped<IValidator<AcknowledgeRequestDto>, AcknowledgeRequestValidator>();
builder.Services.AddScoped<IValidator<AnswerRequestDto>, AnswerRequestValidator>();
builder.Services.AddScoped<IValidator<ChoiceRequestDto>, ChoiceRequestValidator>();
builder.Services.AddScoped<IValidator<CourseDetailRequestDto>, CourseDetailRequestValidator>();
builder.Services.AddScoped<IValidator<EnRollRequestDto>, EnRollRequestValidator>();
builder.Services.AddScoped<IValidator<EvaluationRequestDto>, EvaluationRequestValidator>();
builder.Services.AddScoped<IValidator<EvaluationQuestionRequestDto>, EvaluationQuestionRequestValidator>();
builder.Services.AddScoped<IValidator<InstructorRequestDto>, InstructorRequestValidator>();
builder.Services.AddScoped<IValidator<MethodConRequestDto>, MethodConRequestValidator>();
builder.Services.AddScoped<IValidator<MethodRequestDto>, MethodRequestValidator>();
builder.Services.AddScoped<IValidator<MethodProRequestDto>, MethodProRequestValidator>();
builder.Services.AddScoped<IValidator<MethodRuleConditionRequestDto>, MethodRuleConditionRequestValidator>();
builder.Services.AddScoped<IValidator<MethodRuleRequestDto>, MethodRuleRequestValidator>();
builder.Services.AddScoped<IValidator<ProcessRequestDto>, ProcessRequestValidator>();
builder.Services.AddScoped<IValidator<ProcessTaskRequestDto>, ProcessTaskRequestValidator>();
builder.Services.AddScoped<IValidator<QuestionChoiceRequestDto>, QuestionChoiceRequestValidator>();
builder.Services.AddScoped<IValidator<QuestionRequestDto>, QuestionRequestValidator>();
builder.Services.AddScoped<IValidator<QuizRequestDto>, QuizRequestValidator>();
builder.Services.AddScoped<IValidator<ResourceRequestDto>, ResourceRequestValidator>();
builder.Services.AddScoped<IValidator<ScheduleRequestDto>, ScheduleRequestValidator>();
builder.Services.AddScoped<IValidator<SkillRequestDto>, SkillRequestValidator>();
builder.Services.AddScoped<IValidator<StudyMethodRequestDto>, StudyMethodRequestValidator>();
builder.Services.AddScoped<IValidator<SubjectRequestDto>, SubjectRequestValidator>();
builder.Services.AddScoped<IValidator<TechniqueRequestDto>, TechniqueRequestValidator>();
builder.Services.AddScoped<IValidator<UserAnswerRequestDto>, UserAnswerRequestValidator>();
builder.Services.AddScoped<IValidator<UserQuizAttemptRequestDto>, UserQuizAttemptRequestValidator>();
builder.Services.AddScoped<IValidator<ScheduleSubjectRequestDto>, ScheduleSubjectRequestValidator>();
builder.Services.AddScoped<IValidator<SubjectListRequest>, SubjectListRequestValidator>();
builder.Services.AddScoped<IValidator<ProcessTaskGenerateRequest>, ProcessTaskGenerateRequestValidator>();
builder.Services.AddScoped<IValidator<ListAnswerRequest>, ListAnswerRequestValidator>();
builder.Services.AddScoped<IValidator<SubmitAnswerRequest>, SubmitAnswerRequestValidator>();
builder.Services.AddScoped<IValidator<PaymentRequestDto>, PaymentRequestDtoValidator>();
builder.Services.AddScoped<IValidator<MemberShipPlanRequestDto>, MemberShipPlanRequestValidator>();
builder.Services.AddScoped<IValidator<MemberShipRequestDto>, MemberShipRequestValidator>();
builder.Services.AddScoped<IValidator<FeatureRequestDto>, FeatureRequestValidator>();


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

app.UseMiddleware<ExceptionMiddleware>();

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
