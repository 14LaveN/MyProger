using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyProger.Core.Entity.Account;
using MyProger.Email;
using MyProger.Micro.Identity;
using MyProger.Micro.Identity.Commands.Login;
using MyProger.Micro.Identity.Commands.Register;
using MyProger.Micro.Identity.Common.Entry;
using MyProger.Micro.Identity.Configurations;
using MyProger.Micro.Identity.Database;
using MyProger.Micro.Identity.Extensions;
using MyProger.Micro.Identity.Models.Identity;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(o => 
    o.UseNpgsql(builder.Configuration.GetConnectionString("Db")));

builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssemblies(typeof(LoginCommand).Assembly,
        typeof(LoginCommandHandler).Assembly);
    
    x.RegisterServicesFromAssemblies(typeof(RegisterCommand).Assembly,
        typeof(RegisterCommandHandler).Assembly);
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<EmailService>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<UserManager<AppUser>>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<AppUser, IdentityRole<long>>(options =>
    {
        options.User.RequireUniqueEmail = false;
    })
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAuthentication(opt => {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
            ValidAudiences = new List<string>(){"https://localhost:7124/", "https://localhost:7269/", "https://localhost:7093/", builder.Configuration["Jwt:ValidAudience"]}, // OR "https://localhost:7117/", "https://localhost:7093/"
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
        
                if (string.IsNullOrEmpty(context.Error))
                    context.Error = "invalid_token";
                if (string.IsNullOrEmpty(context.ErrorDescription))
                    context.ErrorDescription = "This request requires a valid JWT access token to be provided";
        
                if (context.AuthenticateFailure == null ||
                    context.AuthenticateFailure.GetType() != typeof(SecurityTokenExpiredException))
                    return context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = context.Error,
                        error_description = context.ErrorDescription
                    }));
                var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                context.Response.Headers.Add("x-token-expired", authenticationException?.Expires.ToString("o"));
                context.ErrorDescription =
                    $"The token expired on {authenticationException?.Expires:o}";
        
                return context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    error = context.Error,
                    error_description = context.ErrorDescription
                }));
            }
        };
    });
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<IAuthorizationHandler, RequireScopeHandler>();

builder.Services.AddSwachbackleService()
    .AddValidators()
    .AddDatabase();

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddDefaultPolicy(corsPolicyBuilder =>
{
    corsPolicyBuilder.WithOrigins("https://localhost:7041").AllowAnyHeader().AllowAnyMethod();
}));

builder.Logging.ClearProviders().SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNLogWeb("nlog.config");

builder.Host.UseNLog();

var app = builder.Build();

// seed scopes
app.UseDataSeeder();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerApp();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();