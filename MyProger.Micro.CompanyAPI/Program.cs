using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using MyProger.Core.Models.MongoSettings;
using MyProger.Micro.CompanyAPI.Command.Company.CreateCompany;
using MyProger.Micro.CompanyAPI.Common.Entry;
using MyProger.Micro.CompanyAPI.Monitoring.Database;
using MyProger.Micro.JobListAPI.Configurations;
using MyProger.Micro.RabbitMQ;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRabbitMq("Company");

builder.Services.AddHealthChecks()
    .AddCheck<CompanyDbHealthCheck>(nameof(CompanyDbHealthCheck));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssemblies(typeof(CreateCompanyCommand).Assembly,
        typeof(CreateCompanyCommandHandler).Assembly);
});

builder.Services.AddDatabase()
    .AddValidators()
    .AddSwachbackleService();

builder.Services.AddAuthentication(config =>
    {
        config.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;
        config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretsecret123456"));

        options.TokenValidationParameters.ValidIssuer = "https://localhost:7017/";
        options.TokenValidationParameters.ValidAudiences
            = new List<string>()
                { "https://localhost:7117/", "https://localhost:7093/" };
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidateLifetime = true;
        options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(5);
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options => options.AddDefaultPolicy(corsPolicyBuilder =>
{
    corsPolicyBuilder.WithOrigins("https://localhost:7124/").AllowAnyHeader().AllowAnyMethod();
}));

builder.Logging.ClearProviders().SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNLogWeb("nlog.config");

builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerApp();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.MapControllers();

app.Run();