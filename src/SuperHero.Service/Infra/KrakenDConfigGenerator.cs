using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperHero.Service.Infra.Configuration;
using System.Text.Json;

namespace SuperHero.Service.Infra;

public interface IKrakenDConfigGenerator
{
    void GenerateConfig();
}

public class KrakenDConfigGenerator : IKrakenDConfigGenerator
{
    private readonly EndpointDataSource _endpointDataSource;
    private readonly ILogger<KrakenDConfigGenerator> _logger;
    private readonly ApplicationOptions _applicationOptions;

    public KrakenDConfigGenerator(EndpointDataSource endpointDataSource, ILogger<KrakenDConfigGenerator> logger, IOptions<ApplicationOptions> options)
    {
        _endpointDataSource = endpointDataSource;
        _logger = logger;
        _applicationOptions = options.Value;
    }

    public void GenerateConfig()
    {
        try
        {
            var endpoints = _endpointDataSource.Endpoints
                .OfType<RouteEndpoint>()
                .Select(endpoint => new
                {
                    endpoint = endpoint.RoutePattern.RawText,
                    method = endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods.FirstOrDefault() ?? "GET",
                    output_encoding = "json",
                    backend = new[]
                    {
                        new
                        {
                            host = new[] { _applicationOptions.Host },
                            url_pattern = endpoint.RoutePattern.RawText,
                            method = "GET"
                        }
                    }
                })
                .ToList();

            if (!endpoints.Any())
            {
                _logger.LogWarning("No endpoints found to include in the KrakenD configuration.");
            }

            var krakendConfig = new
            {
                version = 3,
                endpoints = endpoints
            };

            var json = JsonSerializer.Serialize(krakendConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("krakend.json", json);

            _logger.LogInformation("KrakenD configuration generated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating the KrakenD configuration.");
        }
    }
}
