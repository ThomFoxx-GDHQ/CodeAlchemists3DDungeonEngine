using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoSingleton<PartyManager>
{
    private Character[,] _party = new Character[2, 2];
    [SerializeField] private List<Character> _partyList = new List<Character>();
    [SerializeField] private Transform _characterGroupPanel;
    [SerializeField] private GameObject _characterPanelPrefab;
    private List<GameObject> _panelList = new List<GameObject>();

    public Character[,] Party => _party;
    public int PartySize => _partyList.Count;
    public List<Character> PartyList => _partyList;

    public void AddPartyMember(Character character)
    {
        if (_partyList.Count < 4 && !_partyList.Contains(character))
        {
            _partyList.Add(character);
            GameObject go = Instantiate(_characterPanelPrefab, _characterGroupPanel);
            go.GetComponent<CharacterPanelUI>().AddCharacter(character);
            _panelList.Add(go);
        }
        LoadListToParty(_partyList);
    }

    public void RemovePartyMember(Character character)
    {
        if (_partyList.Contains(character))
        {
            int index = _partyList.IndexOf(character);
            _partyList.Remove(character);
            _panelList.RemoveAt(index);
            LoadListToParty(_partyList);
        }
    }

    public void LoadListToParty(List<Character> list)
    {
        int width = _party.GetLength(0);
        int height = _party.GetLength(1);

        for (int y = 0; y<height; y++)
        {
            for (int x = 0; x<width; x++)
            {
                int index = y*width + x;
                if (index < list.Count)
                    _party[y,x] = list[index];
            }
        }
    }

    public void SaveParty()
    {
        SaveManager.Instance.PartyListToJson( _partyList );
    }

    public void ClearParty()
    {
        _partyList.Clear();
        _panelList.Clear();
        LoadListToParty(_partyList);
    }
}
