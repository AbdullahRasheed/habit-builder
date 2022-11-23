using net_api;
using net_api.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using net_api.Controllers.Context;

var builder = WebApplication.CreateBuilder(args);
var CorsPolicy = "_allowOrigins";

// Add services to the container.

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings")
);

builder.Services.Configure<JwtConfiguration>(
    builder.Configuration.GetSection("JwtSettings")
);

builder.Services.AddScoped<HttpUserService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<LoginService>();
builder.Services.AddSingleton<UserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:3000");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:SecurityKey").Value)),
            ValidateIssuer = false, // May want to change ValidateIssuer and ValidateAudience in the future...
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors(CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
