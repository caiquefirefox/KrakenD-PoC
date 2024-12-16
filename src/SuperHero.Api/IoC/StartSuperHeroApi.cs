using Microsoft.Extensions.Configuration;
using SuperHero.Service.Infra;
using SuperHero.Service.Infra.SuperHero;
using SuperHero.Service.Infra.SuperHero.Configuration;

namespace SuperHero.Api.IoC;

public static class StartSuperHeroApi
{
    private const string _UrlSuperHeroValue = "URL_SUPERHERO_API";
    private const string _TokenSuperHeroValue = "TOKEN_SUPERHERO_API";

    public static IServiceCollection ConfigApi(this IServiceCollection services)
    {
        var heroApiOptions = new HeroApiOptions
        {
            Host = EnvironmentReader.Read<string>(_UrlSuperHeroValue, varEmptyError: "Erro ao ler a variavel de ambiente da API"),
            Token = EnvironmentReader.Read<string>(_TokenSuperHeroValue, varEmptyError: "Erro ao ler a variavel de ambiente de Token da API")
        };

        services.Configure<HeroApiOptions>(options =>
        {
            options.Host = heroApiOptions.Host;
            options.Token = heroApiOptions.Token;
        });

        services.AddHttpClient();
        services.AddTransient<IHeroClientService, HeroClientService>();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole();
        });

        return services;
    }
}
