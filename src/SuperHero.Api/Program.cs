using SuperHero.Api.IoC;
using SuperHero.Service.Infra;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();
builder.Services.ConfigApi(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();

app.UseAuthorization();

app.MapControllers();

// Registrar o evento para gerar o arquivo krakend.json após a aplicação iniciar
var lifetime = app.Lifetime;
lifetime.ApplicationStarted.Register(() =>
{
    var configGenerator = app.Services.GetRequiredService<IKrakenDConfigGenerator>();
    configGenerator.GenerateConfig();
});

app.Run();
