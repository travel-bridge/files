using Files.Application;
using Files.Infrastructure;
using Files.Worker.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<ResizeImageWorker>();
builder.Services.AddHealthChecks();

var app = builder.Build();
app.UseRouting();
app.UseApplication();
app.MapHealthChecks("/health");

await app.RunAsync();