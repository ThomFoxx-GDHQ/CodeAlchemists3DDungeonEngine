using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoSingleton<PartyManager>
{
    private Character[,] _party = new Character[2, 2];
    [SerializeField] private List<Character> _partyList = new List<Character>();

    public Character[,] Party => _party;
    public int PartySize => _partyList.Count;

    public void AddPartyMember(Character character)
    {
        if (_partyList.Count < 4 && !_partyList.Contains(character))
            _partyList.Add(character);
        LoadListToParty();
    }

    public void RemovePartyMember(Character character)
    {

    }

    //For testing
    [ContextMenu("Load List to Party")]
    public void LoadListToParty()
    {
        int width = _party.GetLength(0);
        int height = _party.GetLength(1);

        for (int y = 0; y<height; y++)
        {
            for (int x = 0; x<width; x++)
            {
                int index = y*width + x;
                if (index < _partyList.Count)
                    _party[y,x] = _partyList[index];
            }
        }
    }


}
