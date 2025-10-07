using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuCanvas;

    [SerializeField] InputActionReference _mainMenuAction;

    private void OnEnable()
    {
        _mainMenuAction.action.performed += Action_performed;

    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        _mainMenuCanvas.SetActive(!_mainMenuCanvas.activeInHierarchy);
    }
}
