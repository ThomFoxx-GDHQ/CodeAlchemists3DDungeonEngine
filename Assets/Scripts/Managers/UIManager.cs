using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] GameObject _mainPanel;
    [SerializeField] GameObject _mainMenuPanel;
    [SerializeField] GameObject _partyMenuPanel;
    [SerializeField] GameObject _inventoryPanel;
    [SerializeField] GameObject _compass;

    CompassBehavior _compassBehavior;

    [SerializeField] InputActionReference _mainMenuAction;
    [SerializeField] InputActionReference _selectMenuAction;

    bool _isMainMenuOpen = false;

    private void Start()
    {
        for (int i = 0; i< _mainPanel.transform.childCount; i++)
        {
            _mainPanel.transform.GetChild(i).gameObject.SetActive(false);
        }

        _compassBehavior = _compass.GetComponentInChildren<CompassBehavior>();
    }

    private void OnEnable()
    {
        _mainMenuAction.action.performed += MainMenuAction_performed;
        _selectMenuAction.action.performed += SelectMenuAction_performed;
    }

    private void SelectMenuAction_performed(InputAction.CallbackContext obj)
    {
        if (_isMainMenuOpen) return; //Prevents Menus from being opened with main menu open.

        _partyMenuPanel.SetActive(!_partyMenuPanel.activeInHierarchy);

        if(_inventoryPanel.activeInHierarchy) 
            _inventoryPanel.SetActive(false);
    }

    private void MainMenuAction_performed(InputAction.CallbackContext obj)
    {
        _mainMenuPanel.SetActive(!_mainMenuPanel.activeInHierarchy);
        _isMainMenuOpen = _mainMenuPanel.activeInHierarchy;
    }

    public void UpdateCompass(Directions direction)
    {
        _compassBehavior.SetDirection(direction);
    }
}
