using AdventureGuardian.Models.Models.Domain;
using AdventureGuardian.Models.Models.Enums;

namespace AdventureGuardian.Models.Dto;

public record CreateEncounterDto(int CampaignId, string Name, Creatures[]? Creatures = null, List<int>? CharacterIds = null);