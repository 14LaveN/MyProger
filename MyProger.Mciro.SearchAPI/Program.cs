using Microsoft.Extensions.Options;
using MyProger.Core.Models.Settings;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();