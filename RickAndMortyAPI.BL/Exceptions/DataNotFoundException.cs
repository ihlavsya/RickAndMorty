namespace RickAndMortyAPI.BL.Exceptions;

public class DataNotFoundException:ArgumentException
{
    public DataNotFoundException(string entity, string name)
        : base($"The {entity}'s data with name {name} not found")
    {
    }
    public DataNotFoundException(string url, Exception inner)
        : base($"The data with url {url} not found", inner)
    {
    }

}