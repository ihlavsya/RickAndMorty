using Newtonsoft.Json;

namespace RickAndMortyAPI.BL.Interfaces.Models;

public class PersonDTO
{
    [JsonProperty("name")]
    public string Name { get; init; }
    
    [JsonProperty("status")]
    public string Status { get; init; }
    
    [JsonProperty("species")]
    public string Species { get; init; }
    [JsonProperty("type")]
    public string Type { get; init; }
    
    [JsonProperty("gender")]
    public string Gender { get; init; }
    
    [JsonProperty("origin")]
    public OriginUrlDTO Origin { get; init; }
    
    public FullOriginDTO? FullOrigin { get; set; }
}