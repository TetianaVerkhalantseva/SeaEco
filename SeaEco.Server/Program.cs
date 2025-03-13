using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.GenericRepository;
using SeaEco.Services.AuthServices;
using SeaEco.Services.JwtServices;
using SeaEco.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
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
    };
});

services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration["ConnectionStrings:DefaultConnection"]));
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

services.AddHttpContextAccessor();
services.AddScoped<ICurrentUserContext, CurrentUserContext>();
services.AddTransient<IJwtService, JwtService>();
services.AddTransient<IAuthService, AuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();