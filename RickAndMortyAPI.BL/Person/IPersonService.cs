namespace RickAndMortyAPI.BL;

public interface IPersonService
{
    Task<Person?> GetPerson(string name);
    Task<bool?> CheckPerson(string personName, string episodeName);
}