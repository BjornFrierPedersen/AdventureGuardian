using AdventureGuardian.Infrastructure.Persistance;
using AdventureGuardian.Models.Models;
using AdventureGuardian.Models.Models.ClassModels;
using AdventureGuardian.Models.Models.Enums;
using AdventureGuardian.Models.Models.RaceModels;
using AdventureGuardian.Models.Models.Worlds;
using TinyHeroesRp.Services;
using TinyHeroesRp.Services.Domain;

namespace AdventureGuardian.Infrastructure.Services.Domain;

public class CampaignService
{
    private readonly IOpenAiCommunicatorService _openAiCommunicatorService;
    private readonly WorldService _worldService;
    private readonly CampaignRepository _repository;

    public CampaignService(IOpenAiCommunicatorService openAiCommunicatorService, CampaignRepository repository)
    {
        _openAiCommunicatorService = openAiCommunicatorService;
        _repository = repository;
        _worldService = new WorldService(openAiCommunicatorService);
    }

    public async Task<Campaign> CreateCampaignAsync(string campaignName, string worldName,
        (Gender sex, int age)[] playersByAge,
        World.WorldType worldType, bool displayExplicitContent, string[]? keywords = null)
    {
        var world = await _worldService.GenerateWorldAsync(worldName, playersByAge.Select(_ => _.age).ToArray(),
            worldType, displayExplicitContent, keywords);
        var campaign = new Campaign
        {
            Name = campaignName,
            World = world,
            Characters = playersByAge.Select(player => new Character
            {
                Name = player.sex == Gender.Mand ? "Hr. Danmark" :
                    player.sex == Gender.Kvinde ? "Fru Danmark" : "Hen Danmark",
                Gender = player.sex,
                Race = new Human(),
                Class = new Bonde()
            }).ToList()
        };

        _repository.CreateCampaign(campaign);

        return campaign;
    }

    public Campaign GetCampaign(int id)
    {
        return _repository.GetCampaignById(id);
    }

    public void DeleteCampaign(Campaign campaign)
    {
        //Campaigns.Remove(campaign);
    }

    public Campaign UpdateCampaign(Campaign campaign)
    {
        _repository.UpdateCampaign(campaign);
        return campaign;
    }


    public async Task<Encounter> GenerateEncounterAsync(string name, Campaign campaign,
        List<Character>? characters = null, Creatures[]? creatures = null)
    {
        var encounter = campaign.CreateEncounter(name, creatures);
        var encounterDescription = await _openAiCommunicatorService.SendRequestAsync(encounter.Prompt(characters));
        encounter.Description = encounterDescription;
        campaign.Encounters.Add(encounter);
        return encounter;
    }
}