using Microsoft.AspNetCore.Mvc;
using RickAndMortyAPI.BL;
using RickAndMortyAPI.BL.Exceptions;
using RickAndMortyAPI.ViewModels;

namespace RickAndMortyAPI.Controllers;

[ApiController]
[Route("api/v1")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    [Route("person")]
    public async Task<ActionResult<Person>> Get(string name)
    {
        try
        {
            var person = await _personService.GetPerson(name);
            return person;
        }
        catch (DataNotFoundException e)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Route("check-person")]
    public async Task<ActionResult<bool>> CheckPerson(PersonEpisodeViewModel personEpisode)
    {
        try
        {
            var ifInEpisode = await _personService.CheckPerson(personEpisode.PersonName, personEpisode.EpisodeName);
            return ifInEpisode;
        }
        catch (DataNotFoundException e)
        {
            return NotFound();
        }
    }
}