using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RickAndMortyAPI.BL.Exceptions;
using RickAndMortyAPI.BL.Interfaces.Models;
using RickAndMortyAPI.BL.Interfaces;

namespace RickAndMortyAPI.BL.Externals;

public class HttpPersonProvider : IPersonProvider
{
    private readonly ILogger<HttpPersonProvider> _logger;

    public HttpPersonProvider(ILogger<HttpPersonProvider> logger)
    {
        _logger = logger;
    }

    private async Task<string> GetResponseUrl(string url)
    {
        using HttpClient client = new();
        client.BaseAddress = new Uri("https://rickandmortyapi.com/api/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/rickandmorty"));
        client.DefaultRequestHeaders.Add("User-Agent", "RickAndMorty");
        string json;
        try
        {
            json = await client.GetStringAsync(url);
        }
        catch (HttpRequestException e)
        {
            _logger.LogInformation($"{url} not found", DateTimeOffset.UtcNow);
            throw new DataNotFoundException(url, e);
        }

        return json;
    }

    private async Task<string> GetEpisode(string name)
    {
        var json = await GetResponseUrl(
            $"episode?name={name}");

        return json;
    }

    private async Task<string> GetRickAndMortyCharacter(string name, int page = 1)
    {
        string json = await GetResponseUrl($"character/?page={page}&name={name}");
        return json;
    }

    private async Task<IEnumerable<int>> GetAllEpisodesIdsForName(string name)
    {
        var jsonPersons = await GetRickAndMortyCharacter(name);
        JObject jobjectPersons = JObject.Parse(jsonPersons);
        var info = (JObject)jobjectPersons["info"]!;
        var pages = info["pages"]!.Value<int>();
        var episodes = new List<JToken>();

        for (int i = 2; i < pages; i++)
        {
            var episodesFromPage = ParseResults(jobjectPersons).SelectMany(x => x["episode"]!);
            episodes.AddRange(episodesFromPage);
            jsonPersons = await GetRickAndMortyCharacter(name, i);
        }

        var ids = ParseIds(episodes);
        return ids;
    }

    private async Task<int> GetFirstEpisodeId(string episodeName)
    {
        var jsonEpisode = await GetEpisode(episodeName);
        JObject jobjectEpisodes = JObject.Parse(jsonEpisode);
        var jEpisode = ParseResults(jobjectEpisodes).First!;
        var id = jEpisode["id"]!.Value<int>();
        return id;
    }

    public async Task<bool> CheckPerson(string personName, string episodeName)
    {
        var ids = await GetAllEpisodesIdsForName(personName);
        var id = await GetFirstEpisodeId(episodeName);
        var episodeId = ids.Cast<int?>().FirstOrDefault(index => id == index);

        if (episodeId == null)
        {
            return false;
        }

        return true;
    }

    private IEnumerable<int> ParseIds(IEnumerable<JToken> episodeUrls)
    {
        var ids = new List<int>();
        foreach (var characterUrl in episodeUrls)
        {
            var url = characterUrl.ToString();
            var elements = url.Split(@"/");
            var idStr = elements[elements.Length - 1];
            int id = 0;
            try
            {
                id = Int32.Parse(idStr);
            }
            catch (FormatException ex)
            {
                _logger.LogInformation(ex.Message, DateTimeOffset.UtcNow);
            }

            ids.Add(id);
        }

        return ids;
    }

    private async Task<FullOriginDTO> GetOrigin(string url)
    {
        var origin = await GetResponseUrl(url);
        var fullOrigin = JsonConvert.DeserializeObject<FullOriginDTO>(origin);
        return fullOrigin;
    }

    private JToken ParseResults(JObject json)
    {
        var results = json["results"]!;
        return results;
    }

    public async Task<PersonDTO> GetPerson(string name)
    {
        var json = await GetRickAndMortyCharacter(name);
        var jsonPersons = JObject.Parse(json);
        var jPerson = ParseResults(jsonPersons).First!;
        var personDTO = JsonConvert.DeserializeObject<PersonDTO>(jPerson.ToString());
        personDTO!.FullOrigin = await GetOrigin(personDTO.Origin.Url);
        return personDTO;
    }
}