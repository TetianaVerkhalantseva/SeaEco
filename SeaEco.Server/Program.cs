using System.Security.Claims;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SeaEco.Abstractions.Models.Authentication;
using SeaEco.Abstractions.Models.User;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;
using SeaEco.Reporter.Models;
using SeaEco.Server.Infrastructure;
using SeaEco.Server.Middlewares;
using SeaEco.Services.AuthServices;
using SeaEco.Services.BSurveyService;
using SeaEco.Services.CustomerServices;
using SeaEco.Services.EmailServices;
using SeaEco.Services.EmailServices.Models;
using SeaEco.Services.HashService;
using SeaEco.Services.ImageServices;
using SeaEco.Services.JwtServices;
using SeaEco.Services.ProjectServices;
using SeaEco.Services.SamplingPlanServices;
using SeaEco.Services.TokenServices;
using SeaEco.Services.UserServices;
using SeaEco.Services.Validators;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy
            .WithOrigins(configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()) 
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); 
    });
});

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<RoleAccessorActionFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    })
    .AddFluentValidation();

services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    string key = builder.Configuration.GetSection("JwtOptions:Key").Value ??
                 throw new InvalidOperationException("Credentials not found.");

    byte[] byteKey = Encoding.UTF8.GetBytes(key);
    SecurityKey securityKey = new SymmetricSecurityKey(byteKey);

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = securityKey,
        RoleClaimType = ClaimTypes.Role,
    };
    
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.HttpContext.Request.Cookies["auth_token"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});

// Options
services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
services.Configure<ReportOptions>(configuration.GetSection("ReportOptions"));

services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration["ConnectionStrings:DefaultConnection"]));
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Core services
services.AddHttpContextAccessor();
services.AddScoped<ICurrentUserContext, CurrentUserContext>();
services.AddScoped<IEmailService, SmtpEmailService>();
services.AddTransient<IJwtService, JwtService>();
services.AddTransient<IAuthService, AuthService>();
services.AddTransient<ITokenService, TokenService>();
services.AddTransient<IUserService, UserService>();
services.AddScoped<ICustomerService, CustomerService>();
services.AddScoped<IProjectService, ProjectService>();
services.AddScoped<EmailMessageManager>();
services.AddTransient<IImageService, ImageService>();
services.AddTransient<ISamplingPlanService, SamplingPlanService>();
services.AddTransient<IBSurveyService, BSurveyService>();

// Models validators
services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
services.AddScoped<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();
services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
services.AddScoped<IValidator<ResetPasswordConfirmDto>, ResetPasswordConfirmDtoValidator>();
services.AddScoped<IValidator<EditUserDto>, EditUserDtoValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseMiddleware<AuthMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

try
{
    SeedUser(app.Services);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();

void SeedUser(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    IGenericRepository<Bruker> repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Bruker>>();

    Bruker? admin = repository.GetBy(record => record.Epost == "gruppe202520@gmail.com").GetAwaiter().GetResult();
    if (admin is not null)
    {
        return;
    }
    
    var password = Hasher.Hash("1111");
    admin = new()
    {   
        Id = Guid.NewGuid(),
        Fornavn = "admin",
        Etternavn = "admin",
        Epost = "gruppe202520@gmail.com",
        PassordHash = password.hashed,
        Salt = password.salt,
        IsAdmin = true,
        Aktiv = true
    };
    
    repository.Add(admin).GetAwaiter().GetResult();
}