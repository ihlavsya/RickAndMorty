namespace RickAndMortyAPI.BL;

public class Origin
{
    public string Name { get; init; }
    
    public string Type { get; init; }
    
    public string Dimension { get; init; }

    public Origin(string name, string type, string dimension)
    {
        Name = name;
        Type = type;
        Dimension = dimension;
    }
}