using RickAndMortyAPI.BL.Interfaces.Models;

namespace RickAndMortyAPI.BL.Interfaces;

public interface IPersonProvider
{
    Task<PersonDTO?> GetPerson(string name);
    Task<FullOriginDTO?> GetOrigin(string url);
    Task<bool?> CheckPerson(string personName, string episodeName);
}