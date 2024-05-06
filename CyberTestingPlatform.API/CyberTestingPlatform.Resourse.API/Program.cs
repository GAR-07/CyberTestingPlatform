using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.DataAccess;
using CyberTestingPlatform.Core.Shared;

var builder = WebApplication.CreateBuilder(args);

var authOptions = builder.Configuration.GetSection("AuthOptions").Get<AuthOptions>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ILectureService, LectureService>();
builder.Services.AddScoped<ITestResultService, TestResultService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<ICoursesRepository, CoursesRepository>();
builder.Services.AddScoped<ILecturesRepository, LecturesRepository>();
builder.Services.AddScoped<ITestResultsRepository, TestResultsRepository>();
builder.Services.AddScoped<ITestsRepository, TestsRepository>();
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = 262144000;
    options.MultipartBodyLengthLimit = 262144000;
    options.MemoryBufferThreshold = int.MaxValue;
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // указывает, будет ли валидироваться издатель при валидации токена
        ValidIssuer = authOptions.Issuer, // строка, представляющая издателя
        ValidateAudience = true, // будет ли валидироваться потребитель токена
        ValidAudience = authOptions.Audience, // установка потребителя токена
        ValidateIssuerSigningKey = true, // валидация ключа безопасности
        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(), // установка ключа безопасности
        ValidateLifetime = true, // будет ли валидироваться время существования
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:4200")
            .AllowCredentials()
    );
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddLogging(builder =>
//{
//    builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature.Error is CustomHttpException customHttpException)
        {
            context.Response.StatusCode = customHttpException.StatusCode;
            var errorMessage = exceptionHandlerPathFeature.Error.Message;

            var result = JsonConvert.SerializeObject(new CustomErrorResponse(errorMessage, 422));
            await context.Response.WriteAsync(result);
        }
        else
        {
            // Обработка других типов исключений
            var errorMessage = exceptionHandlerPathFeature.Error.Message;
            var result = JsonConvert.SerializeObject(new CustomErrorResponse(errorMessage, 500));
            await context.Response.WriteAsync(result);
        }
    });
});

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();