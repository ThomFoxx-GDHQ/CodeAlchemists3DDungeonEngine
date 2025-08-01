using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class CharacterCreationUIManager : MonoBehaviour
{
    [SerializeField] TMP_InputField _nameField;
    [SerializeField] TMP_Dropdown _raceDropDown;
    [SerializeField] TMP_Dropdown _classDropdown;

    [SerializeField] TMP_InputField _strengthField;
    [SerializeField] TMP_InputField _agilityField;
    [SerializeField] TMP_InputField _constitutionField;
    [SerializeField] TMP_InputField _fortitudeField;
    [SerializeField] TMP_InputField _wisdomField;
    [SerializeField] TMP_Text _pointsText;
    [SerializeField] private int _pointsTotal;

    string _characterName;
    RaceType _characterRace;
    ClassType _characterClass;
    int _strength = 10;
    int _agility = 10;
    int _constitution = 10;
    int _fortitude = 10;
    int _wisdom= 10;


    List<string> _strings = new List<string>();

    private void Start()
    {
        FillDropDowns();
    }

    private void FillDropDowns()
    {
        _raceDropDown.ClearOptions();
        _classDropdown.ClearOptions();

        foreach (var item in Enum.GetValues(typeof(RaceType)))
            _strings.Add(item.ToString());

        _raceDropDown.AddOptions(_strings);
        RaceChange();

        _strings.Clear();
        foreach (var item in Enum.GetValues(typeof(ClassType)))
            _strings.Add(item.ToString());

        _classDropdown.AddOptions(_strings);
        ClassChange();
    }

    public void NameChange()
    {
        _characterName = _nameField.text;
        DebugLog(_characterName);
    }

    public void RaceChange()
    {
        _characterRace = (RaceType)_raceDropDown.value;
        DebugLog($"Race = {_characterRace.ToString()}");
    }

    public void ClassChange()
    {
        _characterClass = (ClassType)_classDropdown.value;
        DebugLog($"Class = {_characterClass.ToString()}");
    }

    public void DebugLog(string message)
    {
        Debug.Log(message);
    }

    public void AddToStat(string stat)
    {
        if (_pointsTotal <= 0) return;

        switch(stat)
        {
            case "Strength":
                if (_strength >= 20) return;
                _strength++;
                _strengthField.text = _strength.ToString();
                _pointsTotal--;
                _pointsText.text = _pointsTotal.ToString();
                break;
            case "Agility":
                if (_agility >= 20) return;
                _agility++;
                _agilityField.text = _agility.ToString();
                _pointsTotal--;
                _pointsText.text = _pointsTotal.ToString();
                break;
            case "Constitution":
                if (_constitution >= 20) return;
                _constitution++;
                _constitutionField.text = _constitution.ToString();
                _pointsTotal--;
                _pointsText.text = _pointsTotal.ToString();
                break;
            case "Fortitude":
                if (_fortitude >= 20) return;
                _fortitude++;
                _fortitudeField.text = _fortitude.ToString();
                _pointsTotal--;
                _pointsText.text = _pointsTotal.ToString();
                break;
            case "Wisdom":
                if (_wisdom >= 20) return;
                _wisdom++;
                _wisdomField.text = _wisdom.ToString();
                _pointsTotal--;
                _pointsText.text = _pointsTotal.ToString();
                break;
            default:
                Debug.LogError("You Misspelled the Stat.");
                break;
        }
    }

    public void RemoveFromStat(string stat)
    {
        if (_pointsTotal >= 10) return;

        switch (stat)
        {
            case "Strength":
                if (_strength <= 10) return;
                _strength--;
                _strengthField.text = _strength.ToString();
                _pointsTotal++;
                _pointsText.text = _pointsTotal.ToString();
                break;
            case "Agility":
                if (_agility <= 10) return;
                _agility--;
                _agilityField.text = _agility.ToString();
                _pointsTotal++;
                _pointsText.text = _pointsTotal.ToString();
                break;
            case "Constitution":
                if (_constitution <= 10) return;
                _constitution--;
                _constitutionField.text = _constitution.ToString();
                _pointsTotal++;
                _pointsText.text = _pointsTotal.ToString();
                break;
            case "Fortitude":
                if (_fortitude <= 10) return;
                _fortitude--;
                _fortitudeField.text = _fortitude.ToString();
                _pointsTotal++;
                _pointsText.text = _pointsTotal.ToString();
                break;
            case "Wisdom":
                if (_wisdom <= 10) return;
                _wisdom--;
                _wisdomField.text = _wisdom.ToString();
                _pointsTotal++;
                _pointsText.text = _pointsTotal.ToString();
                break;
            default:
                Debug.LogError("You Misspelled the Stat.");
                break;
        }
    }

    public void BuildCharacter()
    {
        Character character = new Character(_characterName, _characterRace, _characterClass, 100, 100, _strength, _agility, _constitution, _fortitude, _wisdom);
        CharacterManager.Instance.AddCharacterToMasterList(character);
        if (PartyManager.Instance.PartySize <4)
            PartyManager.Instance.AddPartyMember(character);

        ClearUI();
    }

    public void ClearUI()
    {
        _nameField.text = string.Empty;
        _strengthField.text = string.Empty;
        _agilityField.text = string.Empty;
        _constitutionField.text = string.Empty;
        _fortitudeField.text = string.Empty;
        _wisdomField .text = string.Empty;
        _raceDropDown.SetValueWithoutNotify(0);
        _classDropdown.SetValueWithoutNotify(0);

        _characterName= string.Empty;
        _characterRace = RaceType.Human;
        _characterClass = ClassType.Warrior;
        _strength = 10;
        _agility = 10;
        _constitution = 10;
        _fortitude = 10;
        _wisdom = 10;
        _pointsTotal = 10;
        _pointsText.text = _pointsTotal.ToString();
    }
}
