using Microsoft.AspNetCore.Mvc;
using SuperHero.Service.Infra.SuperHero;
using SuperHero.Service.Infra.Web;

namespace SuperHero.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuperHeroController : ManagedController
{
    private readonly IHeroClientService _heroClientService;

    public SuperHeroController(IHeroClientService heroClientService, ILogger<ManagedController> logger) : base(logger)
    {
        _heroClientService = heroClientService;
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetHeroByName(string name)
    {
        return await TryExecuteOK(async () => await _heroClientService.GetHeroByName(name));
    }
}
