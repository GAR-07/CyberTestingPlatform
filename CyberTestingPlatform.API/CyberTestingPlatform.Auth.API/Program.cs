using CyberTestingPlatform.Auth.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CyberTestingPlatform.DataAccess;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Application.Models;
using CyberTestingPlatform.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

var authOptions = builder.Configuration.GetSection("AuthOptions").Get<AuthOptions>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // ���������, ����� �� �������������� �������� ��� ��������� ������
        ValidIssuer = authOptions.Issuer, // ������, �������������� ��������
        ValidateAudience = true, // ����� �� �������������� ����������� ������
        ValidAudience = authOptions.Audience, // ��������� ����������� ������
        ValidateIssuerSigningKey = true, // ��������� ����� ������������
        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(), // ��������� ����� ������������
        ValidateLifetime = true, // ����� �� �������������� ����� �������������
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();