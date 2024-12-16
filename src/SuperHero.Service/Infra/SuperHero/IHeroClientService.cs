using SuperHero.Service.Infra.SuperHero.DTO;

namespace SuperHero.Service.Infra.SuperHero;

public interface IHeroClientService
{
    Task<SearchNameResponseDTO> GetHeroByName(string name);
}
