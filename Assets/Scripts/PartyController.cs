using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.EventSystems;

public class PartyController : MonoBehaviour
{
    InputSystem_Actions _input;
    Vector2 _movement;
    Vector3 _moveDirection;
    float _turnDirection;
    float _turn;
    [SerializeField] float _turnAngle = 90f;
    [SerializeField] float _gridTileSize = 2f;
    [SerializeField] float _speed = 5f;
    [SerializeField] float _turnSpeed = 1f;

    Vector3 _startPOS;
    Vector3 _targetPOS;

    private void OnEnable()
    {
        _input = new InputSystem_Actions();
        _input.Player.Enable();
        _input.Player.Move.started += Move_started;
        _input.Player.Turn.started += Turn_started;
    }

    private void Turn_started(InputAction.CallbackContext obj)
    {
        if (!IsAbleToMove()) return;

        //Debug.Log(obj.ToString());
        _turn = obj.ReadValue<float>();

        _turnDirection = _turnAngle * _turn;

        StartCoroutine(TurnRoutine(_turnDirection));
        //transform.Rotate(Vector3.up, _turnAngle * _turn);               
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        if (!IsAbleToMove()) return;

        //Debug.Log(obj.ToString());
        _movement = obj.ReadValue<Vector2>();
        switch (_movement)
        {
            case Vector2 y when _movement.y > 0:
                //transform.Translate(Vector3.forward * _gridTileSize);
                _moveDirection = transform.forward * _gridTileSize;
                break;
            case Vector2 y when _movement.y < 0:
                _moveDirection = (transform.forward * -1) * _gridTileSize;
                break;
            case Vector2 x when _movement.x > 0:
                _moveDirection = transform.right * _gridTileSize;
                break;
            case Vector2 x when _movement.x < 0:
                _moveDirection = (transform.right * -1) * _gridTileSize;
                break;
            default:
                _moveDirection = Vector3.zero;
                break;
        }
        StartCoroutine(MoveRoutine(_moveDirection));
    }

    private void Update()
    {

    }

    IEnumerator MoveRoutine(Vector3 direction)
    {
        _startPOS = transform.position;        
        _targetPOS = transform.position + direction;
        float step;
        while (transform.position != _targetPOS)
        {
            yield return null;
            step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPOS, step);
        }
    }

    private bool IsAbleToMove()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.InputField>() != null) return false;
            if (EventSystem.current.currentSelectedGameObject.GetComponent<TMPro.TMP_InputField>() != null) return false;
        }
        return true;
    }

    IEnumerator TurnRoutine(float turnDirection)
    {
        float currentRotation = 0;
        float step = 0;
        float speed = _turnSpeed;
        if (turnDirection < 0)
            speed *= -1;

        while (Mathf.Abs(currentRotation) < Mathf.Abs(turnDirection))
        {
            yield return null;
            step += speed;

            currentRotation += step;
            transform.Rotate(Vector3.up, step);
        }

        Vector3 rotation = transform.rotation.eulerAngles;
        switch (rotation.y)
        {
            case float y when (y > 350f && y < 10f):
                rotation.y = 0;
                break;
            case float y when (y > 80f && y < 100f):
                rotation.y = 90;
                break;
            case float y when (y > 170f && y < 190f):
                rotation.y = 180;
                break;
            case float y when (y > 260f && y < 280f):
                rotation.y = 270;
                break;
        }
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Environment":
                _targetPOS = _startPOS;
                break;
            case "Enemy":
                _targetPOS = _startPOS;
                Debug.Log($"Player Takes Damage from {other.transform.name}!");
                break;
            case "NPC":
                _targetPOS = _startPOS;
                Debug.Log($"Player bumped into NPC {other.transform.name}.");
                break;
            default:
                break;
        }
        
        //Debug.Log($"Player hit {other.transform.name}");
    }

    private void OnDisable()
    {
        _input.Player.Disable();
        _input.Player.Move.started -= Move_started;
        _input.Player.Turn.started -= Turn_started;
    }
}
