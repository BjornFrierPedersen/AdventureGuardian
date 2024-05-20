using AdventureGuardian.Models.Models.Domain.Worlds;
using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Models.Dto;

public record CreateCampaignDto(string CampaignName, string WorldName, IEnumerable<Player> Players,
    World.WorldType WorldType, bool DisplayExplicitContent, string[]? WorldKeywords = null);
    
    public record Player(Gender Gender, int Age);