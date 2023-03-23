namespace RickAndMortyAPI.BL;

public class Person
{
    public string Name { get; init; }
    public string Status { get; init; }
    public string Species { get; init; }
    public string Type { get; init; }
    public string Gender { get; init; }
    public Origin Origin { get; set; }
}