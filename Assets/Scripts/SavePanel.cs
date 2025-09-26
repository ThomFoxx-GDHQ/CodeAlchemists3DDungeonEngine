using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SavePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _saveNameField;
    [SerializeField] private Toggle[] _saveSlotToggles;
    private int _activeSlotIndex;

    public void SaveFile()
    {
        SaveManager.Instance.SaveParty(_activeSlotIndex, _saveNameField.text);
    }

    public void ChangeActiveSlot(int index)
    {
        if (_saveSlotToggles[index].isOn)
            _activeSlotIndex = index;
    }
}
