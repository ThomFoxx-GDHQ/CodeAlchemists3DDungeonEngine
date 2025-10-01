using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPanel : MonoBehaviour
{
    [SerializeField] private Toggle[] _loadSlotToggles;
    [SerializeField] private TMP_Text[] _loadSlotLabels;
    private int _activeSlotIndex;
    private string[] _loadSlotFiles = new string[3];

    private void OnEnable()
    {
        _loadSlotFiles[0] = PlayerPrefs.GetString("Slot1", string.Empty);
        _loadSlotFiles[1] = PlayerPrefs.GetString("Slot2", string.Empty);
        _loadSlotFiles[2] = PlayerPrefs.GetString("Slot3", string.Empty);

        for (int i =0;  i < _loadSlotFiles.Length; i++)
        {
            if (!string.IsNullOrEmpty(_loadSlotFiles[i]))
                _loadSlotLabels[i].text = _loadSlotFiles[i];
        }

    }

    public void ChangeActiveSlot(int index)
    {
        if (_loadSlotToggles[index].isOn)
        {
            _activeSlotIndex = index;           
        }
    }
}
