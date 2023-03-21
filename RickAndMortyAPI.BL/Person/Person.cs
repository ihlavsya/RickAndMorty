namespace RickAndMortyAPI.BL;

public class Person
{
    public string Name { get; init; }
    public string Status { get; init; }
    public string Species { get; init; }
    public string Type { get; init; }
    public string Gender { get; init; }
    public Origin Origin { get; init; }

    public Person(string name, string status, string species, string type, string gender, Origin origin)
    {
        Name = name;
        Status = status;
        Species = species;
        Type = type;
        Gender = gender;
        Origin = origin;
    }
}