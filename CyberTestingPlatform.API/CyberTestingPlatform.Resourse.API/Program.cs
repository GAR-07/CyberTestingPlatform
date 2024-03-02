using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CyberTestingPlatform.DataAccess;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Application.Models;
using CyberTestingPlatform.DataAccess.Repositories;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var authOptions = builder.Configuration.GetSection("AuthOptions").Get<AuthOptions>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<ICoursesRepository, CoursesRepository>();
builder.Services.AddScoped<ILecturesRepository, LecturesRepository>();
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = 250000000;
    options.MultipartBodyLengthLimit = 250000000;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
