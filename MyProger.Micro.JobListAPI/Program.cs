using System.Reflection;
using System.Text;
using Elastic.Serilog.Sinks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using MyProger.Core.Models.MongoSettings;
using MyProger.Micro.JobListAPI.Commands.AddJob;
using MyProger.Micro.JobListAPI.Commands.AddLikeToJob;
using MyProger.Micro.JobListAPI.Commands.CloseJob;
using MyProger.Micro.JobListAPI.Commands.UpdateJob;
using MyProger.Micro.JobListAPI.Common.Entry;
using MyProger.Micro.JobListAPI.Configurations;
using MyProger.Micro.JobListAPI.Monitoring.Database;
using MyProger.Micro.RabbitMQ;
using NLog.Web;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddLogging();

builder.Services.AddRabbitMq("JobList");

builder.Services.AddHealthChecks()
    .AddCheck<JobDbHealthCheck>(nameof(JobDbHealthCheck));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssemblies(typeof(AddJobCommand).Assembly,
        typeof(AddJobCommandHandler).Assembly);
    
    x.RegisterServicesFromAssemblies(typeof(CloseJobCommand).Assembly,
        typeof(CloseJobCommandHandler).Assembly);
    
    x.RegisterServicesFromAssemblies(typeof(UpdateJobCommand).Assembly,
        typeof(UpdateJobCommandHandler).Assembly);
    
    x.RegisterServicesFromAssemblies(typeof(AddLikeToJobCommand).Assembly,
        typeof(AddLikeToJobCommandHandler).Assembly);
});

builder.Services.Configure<Settings>(
    builder.Configuration.GetSection("MongoConnection"));

builder.Services.AddDatabase(builder.Configuration)
    .AddValidators()
    .AddSwachbackleService()
    .AddMonitoring();

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

app.UseHttpMetrics();

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

app.UseMetricServer(5000, "/prometheus");

app.MapControllers();

app.Run();