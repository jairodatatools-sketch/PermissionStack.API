using Elastic.Clients.Elasticsearch;

using Microsoft.EntityFrameworkCore;

using PermissionStack.Application.Commands;
using PermissionStack.Application.Handlers;
using PermissionStack.Application.Interfaces;
using PermissionStack.Infrastructure.Persistence;
using PermissionStack.Infrastructure.Services;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RequestPermissionCommand).Assembly));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RequestPermissionHandler).Assembly));


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IElasticService, ElasticService>();
builder.Services.AddScoped<IPermissionEventPublisher, KafkaProducerService>();
builder.Services.AddScoped<KafkaProducerService>();


var elasticSettings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
    .DefaultIndex("permissions");

var elasticClient = new ElasticsearchClient(elasticSettings);


builder.Services.AddSingleton(elasticClient);
builder.Services.AddSingleton<IPermissionEventPublisher, KafkaProducerService>();

WebApplication app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PermissionStack.API v1");
    c.RoutePrefix = "swagger";
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
