using Newtonsoft.Json;

namespace RickAndMortyAPI.BL.Interfaces.Models;

public class OriginUrlDTO
{
    [JsonProperty("name")]
    public string Name { get; init; }
    [JsonProperty("url")]
    public string Url { get; init; }

    public OriginUrlDTO(string name, string url)
    {
        Name = name;
        Url = url;
    }
}