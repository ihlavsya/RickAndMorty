using Microsoft.Extensions.Caching.Memory;
using RickAndMortyAPI.BL.Interfaces;
using RickAndMortyAPI.BL.Interfaces.Models;

namespace RickAndMortyAPI.BL.Externals;

public class CachePersonDecorator : BasePersonProviderDecorator
{
    private readonly IPersonProvider _personProvider;
    private readonly IMemoryCache _getPersonMemoryCache;
    private readonly IMemoryCache _checkPersonMemoryCache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    public CachePersonDecorator(IPersonProvider personProvider, IMemoryCache getPersonMemoryCache,
        IMemoryCache checkPersonMemoryCache) : base(personProvider)
    {
        _personProvider = personProvider;
        _getPersonMemoryCache = getPersonMemoryCache;
        _checkPersonMemoryCache = checkPersonMemoryCache;
        _cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(1));
    }

    public override async Task<bool?> CheckPerson(string personName, string episodeName)
    {
        Tuple<string, string> personAndEpisodeName = new Tuple<string, string>(personName, episodeName);
        if (!_checkPersonMemoryCache.TryGetValue(personAndEpisodeName, out bool? ifInEpisode))
        {
            ifInEpisode = await _personProvider.CheckPerson(personName, episodeName);

            _checkPersonMemoryCache.Set(personAndEpisodeName, ifInEpisode, _cacheEntryOptions);
        }

        return ifInEpisode;
    }

    public override async Task<PersonDTO?> GetPerson(string name)
    {
        if (!_getPersonMemoryCache.TryGetValue(name, out PersonDTO? personDTO))
        {
            personDTO = await _personProvider.GetPerson(name);

            _getPersonMemoryCache.Set(name, personDTO, _cacheEntryOptions);
        }

        return personDTO;
    }
}