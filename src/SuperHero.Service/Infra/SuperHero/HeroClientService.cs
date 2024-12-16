using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperHero.Service.Infra.SuperHero.Configuration;
using SuperHero.Service.Infra.SuperHero.DTO;
using System.Text.Json;

namespace SuperHero.Service.Infra.SuperHero;

public class HeroClientService : IHeroClientService

{
    private readonly IHttpClientFactory _clientFactory;
    private readonly HeroApiOptions _apiOptions;
    private readonly ILogger<HeroClientService> _logger;

    public HeroClientService(IHttpClientFactory clientFactory, IOptions<HeroApiOptions> apiOptions, ILogger<HeroClientService> logger)
    {
        _clientFactory = clientFactory;
        _apiOptions = apiOptions.Value;
        _logger = logger;
    }

    private string GetUrlHeroByName(string name)
    {
        return $"{_apiOptions.Host}/{_apiOptions.Token}/search/{name}";
    }

    public async Task<SearchNameResponseDTO> GetHeroByName(string name)
    {
        var client = _clientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, GetUrlHeroByName(name));

        try
        {
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var convertedResult = JsonSerializer.Deserialize<SearchNameResponseDTO>(result) ?? new SearchNameResponseDTO();
                return convertedResult;
            }
            else
            {
                _logger.LogWarning("Request to {Url} failed with status code {StatusCode}", GetUrlHeroByName(name), response.StatusCode);
                return new SearchNameResponseDTO();
            }
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "An error occurred while sending the HTTP request to {Url}", GetUrlHeroByName(name));
            throw new Exception("An error occurred while sending the HTTP request.", httpEx);
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "An error occurred while deserializing the response from {Url}", GetUrlHeroByName(name));
            throw new Exception("An error occurred while deserializing the response.", jsonEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while processing the request to {Url}", GetUrlHeroByName(name));
            throw new Exception("An unexpected error occurred.", ex);
        }
    }
}