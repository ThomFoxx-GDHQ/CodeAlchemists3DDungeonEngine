using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

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
    [SerializeField] TMP_Text _points;

    string _characterName;
    RaceType _characterRace;
    ClassType _characterClass;
    int _strength, _agility, _constitution, _fortitude, _wisdom;

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
}
