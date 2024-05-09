using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Diagnostics;
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
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccessControlService, AccessControlService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IAccessControlsRepository, AccessControlsRepository>();
builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
builder.Services.AddScoped<ICoursesRepository, CoursesRepository>();
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

        var errorMessage = exceptionHandlerPathFeature.Error.Message;
        await context.Response.WriteAsync(errorMessage);
    });
});

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();