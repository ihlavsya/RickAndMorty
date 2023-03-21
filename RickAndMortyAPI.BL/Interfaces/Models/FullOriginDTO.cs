using Newtonsoft.Json;

namespace RickAndMortyAPI.BL.Interfaces.Models;

public class FullOriginDTO
{
    [JsonProperty("name")]
    public string Name { get; init; }
    
    [JsonProperty("type")]
    public string Type { get; init; }
    
    [JsonProperty("dimension")]
    public string Dimension { get; init; }

    public FullOriginDTO(string name, string type, string dimension)
    {
        Name = name;
        Type = type;
        Dimension = dimension;
    }
}