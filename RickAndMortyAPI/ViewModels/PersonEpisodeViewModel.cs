namespace RickAndMortyAPI.ViewModels;

public class PersonEpisodeViewModel
{
    public string PersonName { get; init; }
    public string EpisodeName { get; init; }

    public PersonEpisodeViewModel(string personName, string episodeName)
    {
        PersonName = personName;
        EpisodeName = episodeName;
    }
}