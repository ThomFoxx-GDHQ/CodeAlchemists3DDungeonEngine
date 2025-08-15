using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCharacterPanelUI : MonoBehaviour
{
    [SerializeField] Transform _contentTransform;
    [SerializeField] GameObject _characterInfoPrefab;
    GameObject _infoPanel;
    ShortCharacterInfoUI _characterInfoUI;
    List<Button> _buttons = new List<Button>();

    private void OnEnable()
    {
        if (_contentTransform.childCount > 0)
        {
            int childs = _contentTransform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                GameObject.Destroy(_contentTransform.GetChild(i).gameObject);
            }
        }

        _buttons.Clear();
        foreach (Character character in CharacterManager.Instance.MasterCharacterList)
        {
            _infoPanel = Instantiate(_characterInfoPrefab, _contentTransform);
            _characterInfoUI = _infoPanel.GetComponent<ShortCharacterInfoUI>();

            _characterInfoUI.LoadInfo(character);
            _buttons.Add(_infoPanel.GetComponent<Button>());
        }
        PartyCheck();
    }

    public void PartyCheck()
    {
        Debug.Log("Checking Party Size");

        if (PartyManager.Instance.PartySize >= 4)
            foreach (Button button in _buttons)
                button.interactable = false;
        else
            foreach (Button button in _buttons)
            {
                if (!PartyManager.Instance.PartyList.Contains(button.GetComponent<ShortCharacterInfoUI>().Character))
                {
                    button.interactable = true;
                }
            }
    }
}
