using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShortCharacterInfoUI : MonoBehaviour
{
    Character _character;
    [SerializeField] Image _portrait;
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _classText;
    [SerializeField] TMP_Text _raceText;
    Button _button;

    public Character Character => _character;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
        {
            AddToParty();
            GetComponentInParent<LoadCharacterPanelUI>().PartyCheck();
        });
    }

    public void LoadInfo(Character character)
    {
        _character = character;
        _nameText.text = _character.name;
        _classText.text = _character.JobType.ToString();
        _raceText.text = _character.Race.ToString();
        _portrait.sprite = PortraitManager.Instance.GetPortrait(_character.PortraitID);
        _character.LoadInventory();
    }

    public void AddToParty()
    {
        PartyManager.Instance.AddPartyMember(_character);
        _button.interactable = false;
    }
}
