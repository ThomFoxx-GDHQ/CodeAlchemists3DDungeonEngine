using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoSingleton<CharacterManager>
{
    private string[] _randomNames = new string[] 
    { "Aelric", "Brivana", "Caldus", "Daenira", "Elowen", "Faelar", "Galdrin", "Hestara", "Ildric", "Joreth", "Kaelis", "Lysira", "Maelor", "Nyxen", "Orindra", "Padrik", "Quenryl", "Rhovan", "Sylvar", "Thalira", "Urien", "Vaelis", "Wynna", "Xandor", "Yllara", "Zevrik", "Anwen", "Baelor", "Cyrille", "Doreth", "Eryndor", "Fenara", "Garek", "Halindra", "Iskren", "Jastiel", "Kaeric", "Liora", "Myrren", "Narthas", "Olyssia", "Penric", "Quorra", "Rythen", "Seraphin", "Talryn", "Ustiel", "Vireth", "Wrenna", "Xavren", "Ysolde", "Zareth", "Aemara", "Brannik", "Cindral", "Draziel", "Elsinne", "Faenric", "Gwyra", "Harkan", "Isilme", "Jorek", "Kalindra", "Luthien", "Morven", "Nymeri", "Othric", "Prynn", "Quelan", "Rydan", "Sarella", "Taelen", "Ulyra", "Vornik", "Wystan", "Xalara", "Yrren", "Zirael", "Aelar", "Brynja", "Corvus", "Dravyn", "Elaria", "Fendrel", "Gilwen", "Hrothic", "Inessa", "Jaelis", "Korran", "Leoril", "Myrra", "Norric", "Oswyn", "Pyrris", "Quindra", "Rowan", "Selric", "Thalara" };

    [SerializeField] private List<Character> _masterCharacterList = new List<Character>();
    private PartyManager _partyManager;

    public List<Character> MasterCharacterList => _masterCharacterList;

    private void Start()
    {
        _partyManager = GetComponent<PartyManager>();
    }

    [ContextMenu("Create Random Character")]
    public void CreateRandomCharacter()
    {
        string name = GetName();
        int race = Random.Range(0, System.Enum.GetValues(typeof(RaceType)).Length);
        int jobType = Random.Range(0, System.Enum.GetValues(typeof(ClassType)).Length);
        Character character = new Character(name, (RaceType)race,(ClassType)jobType);
        
        _masterCharacterList.Add(character);

        _partyManager.AddPartyMember(character);
    }

    private string GetName()
    {
        return _randomNames[Random.Range(0,_randomNames.Length)];
    }
    
    public void AddCharacterToMasterList(Character character)
    {
        _masterCharacterList.Add(character);
        SaveManager.Instance.CharacterListToJson(_masterCharacterList);
    }

    public void ReloadMasterList(List<Character> characterList)
    {
        _masterCharacterList = characterList;
    }
}
