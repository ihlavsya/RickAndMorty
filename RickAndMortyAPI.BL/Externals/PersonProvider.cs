using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RickAndMortyAPI.BL.Interfaces.Models;
using RickAndMortyAPI.BL.Interfaces;

namespace RickAndMortyAPI.BL.Externals;

public class PersonProvider : IPersonProvider
{
    private async Task<string?> GetResponseUrl(string url)
    {
        using HttpClient client = new();
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
            Console.WriteLine($"{url} not found");
            return null;
        }

        return json;
    }
    public async Task<string?> GetEpisode(string name)
    {
        var json = await GetResponseUrl(
                $"https://rickandmortyapi.com/api/episode?name={name}");

        return json;
    }
    
    private async Task<string?> GetRickAndMortyCharacter(string name)
    {
        string? json = await GetResponseUrl($"https://rickandmortyapi.com/api/character/?name={name}");
        return json;
    }
    public async Task<bool?> CheckPerson(string personName, string episodeName)
    {
        string? jsonPersons = await GetRickAndMortyCharacter(personName);
        if (jsonPersons == null)
        {
            return null;
        }
        JObject jobjectPersons = JObject.Parse(jsonPersons);
        var episodes = jobjectPersons["results"]!.SelectMany(x => x["episode"]!);
        var ids = ParseIds(episodes);
        var jsonEpisode = await GetEpisode(episodeName);
        if (jsonEpisode == null)
        {
            return null;
        }
        JObject jobjectEpisodes = JObject.Parse(jsonEpisode);
        JObject jEpisode = (JObject)jobjectEpisodes["results"]!.First!;
        var id = Int32.Parse(jEpisode["id"]!.ToString());
        int? episodeId = ids.Cast<int?>().FirstOrDefault(index => id == index);
        
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
            int id= 0;
            try
            {
                id = Int32.Parse(idStr);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            ids.Add(id);
        }

        return ids;
    }

    public async Task<FullOriginDTO?> GetOrigin(string url)
    {
        var origin = await GetResponseUrl(url);
        if (origin == null)
        {
            return null;
        }
        var fullOrigin = JsonConvert.DeserializeObject<FullOriginDTO>(origin);
        return fullOrigin;
    }

    public async Task<PersonDTO?> GetPerson(string name)
    {
        var json = await GetRickAndMortyCharacter(name);
        if (json == null)
        {
            return null;
        }
        var jsonPersons = JObject.Parse(json);
        var jPerson = (JObject)jsonPersons["results"]!.First!;
        var personDTO = JsonConvert.DeserializeObject<PersonDTO>(jPerson.ToString());
        personDTO!.FullOrigin = await GetOrigin(personDTO.Origin.Url);
        return personDTO;
    }
}