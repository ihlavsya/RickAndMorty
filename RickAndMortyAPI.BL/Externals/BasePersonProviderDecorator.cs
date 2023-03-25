using RickAndMortyAPI.BL.Interfaces;
using RickAndMortyAPI.BL.Interfaces.Models;

namespace RickAndMortyAPI.BL.Externals;

public class BasePersonProviderDecorator : IPersonProvider
{
    private readonly IPersonProvider _personProvider;
    public BasePersonProviderDecorator(IPersonProvider personProvider)
    {
        _personProvider = personProvider;
    }

    public virtual async Task<PersonDTO?> GetPerson(string name)
    {
        var personDto = await _personProvider.GetPerson(name);
        return personDto;
    }
    
    public virtual async Task<bool?> CheckPerson(string personName, string episodeName)
    {
        var ifInEpisode = await _personProvider.CheckPerson(personName, episodeName);
        return ifInEpisode;
    }
}