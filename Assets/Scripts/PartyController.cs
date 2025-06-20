using UnityEngine;
using UnityEngine.InputSystem;

public class PartyController : MonoBehaviour
{
    InputSystem_Actions _input;
    Vector2 _movement;
    float _turn;
    [SerializeField] float _turnAngle = 90f;
    [SerializeField] float _gridTileSize = 2f;

    private void OnEnable()
    {
        _input = new InputSystem_Actions();
        _input.Player.Enable();
        _input.Player.Move.started += Move_started;
        _input.Player.Turn.started += Turn_started;
    }

    private void Turn_started(InputAction.CallbackContext obj)
    {
        //Debug.Log(obj.ToString());
        _turn = obj.ReadValue<float>();

        transform.Rotate(Vector3.up, _turnAngle * _turn);
               
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        Debug.Log(obj.ToString());
        _movement = obj.ReadValue<Vector2>();
        switch (_movement)
        {
            case Vector2 y when _movement.y > 0:
                transform.Translate(Vector3.forward * _gridTileSize);
                break;
            case Vector2 y when _movement.y < 0:
                transform.Translate(Vector3.back * _gridTileSize);
                break;
            case Vector2 x when _movement.x > 0:
                transform.Translate(Vector3.right * _gridTileSize);
                break;
            case Vector2 x when _movement.x < 0:
                transform.Translate(Vector3.left * _gridTileSize);
                break;
            default:
                break;
        }
    }

    private void Update()
    {

    }

    private void OnDisable()
    {
        _input.Player.Disable();
    }
}
