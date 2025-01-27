using EShop.API.Filters;
using EShop.API.Middlewares;
using EShop.Application;
using EShop.Application.Mailing;
using EShop.Application.Security.Token;
using EShop.Infrastructure;
using EShop.Infrastructure.Security;
using EShop.Infrastructure.Storages.Cloudinary;
using EShop.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using System.Text;

/*
 * Authentication - Yes
 * Authorization - Yes
 * Validation - Yes
 * Logging - Yes
 * Caching - No
 * Transaction - Yes
 */

var builder = WebApplication.CreateBuilder(args);

#region Serilog Configuration

using var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File(new Serilog.Formatting.Json.JsonFormatter(),
        path: "logs/log.json",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(logger);
#endregion

#region Configure Options/AppSettings

IConfigurationSection tokenSection = builder.Configuration.GetSection("JwtOptions");
// Add to the IoC container.
builder.Services.Configure<TokenOptions>(tokenSection);
builder.Services.Configure<CloudinaryOptions>(builder.Configuration.GetSection("CloudinaryOptions"));
builder.Services.Configure<MailOptions>(builder.Configuration.GetSection("MailOptions"));

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
#endregion

#region Add Services

builder.Services.AddApplicationServices();

builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddInfrastructureServices()
    .AddStorage<CloudinaryStorage>();

#endregion

#region ASP.NET

// validate modelstate with FluentValidation
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
})
    // suppress default validation filter
.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithExposedHeaders("location");
}));
#endregion

#region Security Configuration

TokenOptions tokenOptions = tokenSection.Get<TokenOptions>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidAudience = tokenOptions.Audience,
            ValidIssuer = tokenOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            LifetimeValidator = (_, expires, _, _) => expires != null && expires > DateTime.UtcNow
        };
    });
#endregion

#region Swagger Configuration

builder.Services.AddSwaggerGen(setup =>
{
    setup.AddSecurityDefinition("JWT Authorization",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Enter the `Generated-JWT-Token`:",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
        });

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
         {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "JWT Authorization", Type = ReferenceType.SecurityScheme }
            },
            Array.Empty<string>()
        }
    });
});
#endregion

var app = builder.Build();

#region Use Middlewares

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseStaticFiles();

app.UseHttpsRedirection();

var enrichSerilogContextBeforeHandler = delegate (HttpContext context)
{
    LogContext.PushProperty("IpAddress", context.Connection.RemoteIpAddress);

    var user = context.User;
    if (user.Identity?.IsAuthenticated ?? false)
        LogContext.PushProperty("User", new
        {
            user.Identity.Name,
            Email = user.Claims.GetEmail()
        }, true);
};

app.UseMiddleware<ExceptionHandlerMiddleware>(enrichSerilogContextBeforeHandler);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
