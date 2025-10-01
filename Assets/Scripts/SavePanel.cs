using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SavePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _saveNameField;
    [SerializeField] private Toggle[] _saveSlotToggles;
    private int _activeSlotIndex;
    private string[] _saveSlotFiles = new string[3];

    private void OnEnable()
    {
        _saveSlotFiles[0] = PlayerPrefs.GetString("Slot1", string.Empty);
        _saveSlotFiles[1] = PlayerPrefs.GetString("Slot2", string.Empty);
        _saveSlotFiles[2] = PlayerPrefs.GetString("Slot3", string.Empty);
    }

    public void SaveFile()
    {
        SaveManager.Instance.SaveParty(_activeSlotIndex, _saveNameField.text);
        switch (_activeSlotIndex)
        {
            case 0:
                PlayerPrefs.SetString("Slot1", _saveNameField.text);                
                break;
            case 1:
                PlayerPrefs.SetString("Slot2", _saveNameField.text);
                break;
            case 2:
                PlayerPrefs.SetString("Slot3", _saveNameField.text);
                break;
            default:
                break;
        }
        PlayerPrefs.Save();
        _saveSlotFiles[_activeSlotIndex] = _saveNameField.text;
    }

    public void ChangeActiveSlot(int index)
    {
        if (_saveSlotToggles[index].isOn)
        {
            _activeSlotIndex = index;
            if (!string.IsNullOrEmpty(_saveSlotFiles[index]))
                _saveNameField.text = _saveSlotFiles[index];
            else _saveNameField.text = string.Empty;
        }
    }
}
