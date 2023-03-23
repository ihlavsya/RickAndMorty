using RickAndMortyAPI.BL.Interfaces.Models;

namespace RickAndMortyAPI.BL.Interfaces;

public interface IPersonProvider
{
    Task<PersonDTO?> GetPerson(string name);
    Task<bool?> CheckPerson(string personName, string episodeName);
}