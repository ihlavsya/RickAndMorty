using AutoMapper;
using RickAndMortyAPI.BL.Interfaces;
using RickAndMortyAPI.BL.Interfaces.Models;

namespace RickAndMortyAPI.BL;

public class PersonService : IPersonService
{
    private readonly IPersonProvider _personProvider;
    private readonly IMapper _mapper;

    public PersonService(IPersonProvider personProvider, IMapper mapper)
    {
        _personProvider = personProvider;
        _mapper = mapper;
    }

    public async Task<Person?> GetPerson(string name)
    {
        PersonDTO? personDTO = await _personProvider.GetPerson(name);
        var person = _mapper.Map<Person>(personDTO);
        return person;
    }

    public async Task<bool?> CheckPerson(string personName, string episodeName)
    {
        var ifInEpisode = await _personProvider.CheckPerson(personName, episodeName);
        return ifInEpisode;
    }

}