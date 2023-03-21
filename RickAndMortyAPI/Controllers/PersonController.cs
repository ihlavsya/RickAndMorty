using Microsoft.AspNetCore.Mvc;
using RickAndMortyAPI.BL;
using RickAndMortyAPI.ViewModels;

namespace RickAndMortyAPI.Controllers;

[ApiController]
[Route("api/v1")]
public class PersonController : ControllerBase
{
    private readonly ILogger<PersonController> _logger;
    private readonly IPersonService _personService;

    public PersonController(ILogger<PersonController> logger, IPersonService personService)
    {
        _logger = logger;
        _personService = personService;
    }

    [HttpGet]
    [Route("person")]
    public async Task<ActionResult<Person>> Get(string name)
    {
        var person = await _personService.GetPerson(name);
        if (person == null)
        {
            return NotFound();
        }
        return person;
    }
    
    [HttpPost]
    [Route("check-person")]
    public async Task<ActionResult<bool>> CheckPerson(PersonEpisodeViewModel personEpisode)
    {
        var ifInEpisode = await _personService.CheckPerson(personEpisode.PersonName, personEpisode.EpisodeName);
        if (ifInEpisode == null)
        {
            return NotFound();
        };
        return ifInEpisode;
    }
}