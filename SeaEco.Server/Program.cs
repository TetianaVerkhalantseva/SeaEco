using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SeaEco.Abstractions.Models.Authentication;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.GenericRepository;
using SeaEco.Server.Middlewares;
using SeaEco.Services.AuthServices;
using SeaEco.Services.CustomerServices;
using SeaEco.Services.EmailServices;
using SeaEco.Services.EmailServices.Models;
using SeaEco.Services.JwtServices;
using SeaEco.Services.TokenServices;
using SeaEco.Services.UserServices;
using SeaEco.Services.Validators;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => 
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    })
    .AddFluentValidation();

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
    };
});

services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));

services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration["ConnectionStrings:DefaultConnection"]));
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Core services
services.AddHttpContextAccessor();
services.AddScoped<ICurrentUserContext, CurrentUserContext>();
services.AddScoped<IEmailService, SmtpEmailService>();
services.AddTransient<IJwtService, JwtService>();
services.AddTransient<IAuthService, AuthService>();
services.AddTransient<ITokenService, TokenService>();

// Register CustomerService
services.AddScoped<ICustomerService, CustomerService>();

// Models validators
services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
services.AddScoped<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();
services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
services.AddScoped<IValidator<ResetPasswordConfirmDto>, ResetPasswordConfirmDtoValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<AuthMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();