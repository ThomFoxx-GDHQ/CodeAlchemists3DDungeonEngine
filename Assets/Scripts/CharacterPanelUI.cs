using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelUI : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _raceText;
    [SerializeField] private TMP_Text _classText;
    [SerializeField] private Image _portrait;

    [Header("Vitals (use either Fill Images or Sliders)")]
    [SerializeField] private Image _healthFill;   // Image.type = Filled
    [SerializeField] private Image _manaFill;     // Image.type = Filled
   
    [Header("Stat Labels (left column names, right column values)")]
    [SerializeField] private TMP_Text _strengthValue;
    [SerializeField] private TMP_Text _agilityValue;
    [SerializeField] private TMP_Text _fortitudeValue;
    [SerializeField] private TMP_Text _wisdomValue;
    [SerializeField] private TMP_Text _constitutionValue;

    [Header("Optional Radar")]
    [SerializeField] private SimpleRadialStats _radarComponent;

    [SerializeField] private Character _character;

    public void AddCharacter(Character character)
    {
        _character = character;
        UpdatePanelInfo();
    }

    public void RemoveFromParty()
    {
        PartyManager.Instance.RemovePartyMember(_character);
        Destroy(this.gameObject);
    }

    [ContextMenu("Update Character Panel")]
    public void UpdatePanelInfo()
    {
        _nameText.text = _character.name;
        _raceText.text = _character.Race.ToString();
        _classText.text = _character.JobType.ToString();
        _portrait.sprite = PortraitManager.Instance.GetPortrait(_character.PortraitID);
        Debug.Log($"Character Health bar:{_character.CurrentHealth}/{_character.HealthPoints} = {(float)_character.CurrentHealth / _character.HealthPoints}");
        _healthFill.fillAmount = (float)_character.CurrentHealth / _character.HealthPoints;
        _manaFill.fillAmount = (float)_character.CurrentMagic / _character.MagicPoints;
        _strengthValue.text = _character.Strength.ToString();
        _agilityValue.text = _character.Agility.ToString();
        _fortitudeValue.text = _character.Fortitude.ToString();
        _wisdomValue.text = _character.Wisdom.ToString();
        _constitutionValue.text = _character.Constitution.ToString();

        _radarComponent.SetStat(Stats.Strength, _character.Strength);
        _radarComponent.SetStat(Stats.Agility, _character.Agility);
        _radarComponent.SetStat(Stats.Fortitude, _character.Fortitude);
        _radarComponent.SetStat(Stats.Wisdom, _character.Wisdom);
        _radarComponent.SetStat(Stats.Constitution, _character.Constitution);
    }
}
