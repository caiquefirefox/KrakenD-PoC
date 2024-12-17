using SuperHero.Service.Infra;
using SuperHero.Service.Infra.Configuration;
using SuperHero.Service.Infra.SuperHero;
using SuperHero.Service.Infra.SuperHero.Configuration;

namespace SuperHero.Api.IoC;

public static class StartSuperHeroApi
{
    private const string _UrlSuperHeroValue = "URL_SUPERHERO_API";
    private const string _TokenSuperHeroValue = "TOKEN_SUPERHERO_API";

    public static IServiceCollection ConfigApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HeroApiOptions>(configuration.GetSection("HeroApi"));
        services.Configure<ApplicationOptions>(configuration.GetSection("Application"));

        services.AddHttpClient();
        services.AddTransient<IHeroClientService, HeroClientService>();
        services.AddSingleton<IKrakenDConfigGenerator, KrakenDConfigGenerator>();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole();
        });

        return services;
    }
}
