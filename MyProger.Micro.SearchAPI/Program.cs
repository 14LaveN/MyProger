using Microsoft.Extensions.Options;
using MyProger.Core.Models.Settings;
using MyProger.Mciro.SearchAPI.Command.Search.SearchJob;
using MyProger.Mciro.SearchAPI.Common.Entry;
using MyProger.Mciro.SearchAPI.Configurations;
using MyProger.Mciro.SearchAPI.Service;
using Nest;
using NLog.Web;
using LogLevel = NLog.LogLevel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISearchService, SearchService>();

builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssemblies(typeof(SearchJobCommand).Assembly,
        typeof(SearchJobCommandHandler).Assembly);
});

builder.Services.AddSwachbackleService()
    .AddValidators()
    .AddMonitoring(builder.Environment);

builder.Services.AddLogging(options =>
{
    options.ClearProviders()
        .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
});

builder.Logging.AddNLogWeb("nlog.config");

builder.Host.UseNLog();


builder.Services.AddTransient<SearchService>();

builder.Services.Configure<ElasticSearchSettings>(builder.Configuration.GetSection("ElasticSearchSettings"));

builder.Services.AddSingleton<ElasticClient>(provider =>
{
    var settings = provider.GetService<IOptions<ElasticSearchSettings>>()?.Value;
    var node = new Uri(settings!.Url);
    var connectionSettings = new ConnectionSettings(node).DefaultIndex(settings.DefaultIndex);

    return new ElasticClient(connectionSettings);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPrometheusScrapingEndpoint();

app.UseOpenTelemetryPrometheusScrapingEndpoint("/prometheus");

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();