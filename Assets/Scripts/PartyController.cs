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
    bool _isMoving = false;

    private void OnEnable()
    {
        _input = new InputSystem_Actions();
        _input.Player.Enable();
        _input.Player.Move.started += Move_started;
        _input.Player.Turn.started += Turn_started;
    }

    private void Turn_started(InputAction.CallbackContext obj)
    {
        if (!IsAbleToMove() || _isMoving) return;

        //Debug.Log(obj.ToString());
        _turn = obj.ReadValue<float>();

        _turnDirection = _turnAngle * _turn;

        StartCoroutine(TurnRoutine(_turnDirection));
        //transform.Rotate(Vector3.up, _turnAngle * _turn);               
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        if (!IsAbleToMove() || _isMoving) return;

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
    
    IEnumerator MoveRoutine(Vector3 direction)
    {
        _isMoving = true;
        _startPOS = TruncateVector3(transform.position);
        _targetPOS = _startPOS + direction;
        float step;
        while (transform.position != _targetPOS)
        {
            yield return null;
            step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPOS, step);
        }
        _isMoving = false;
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
        _isMoving = true;
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
        Directions rotationDirection = Directions.North;
        switch (rotation.y)
        {
            case float y when (y > 350f && y < 10f):
                rotation.y = 0;
                rotationDirection = Directions.North;
                break;
            case float y when (y > 80f && y < 100f):
                rotation.y = 90;
                rotationDirection = Directions.East;
                break;
            case float y when (y > 170f && y < 190f):
                rotation.y = 180;
                rotationDirection = Directions.South;
                break;
            case float y when (y > 260f && y < 280f):
                rotation.y = 270;
                rotationDirection = Directions.West;
                break;
        }
        transform.rotation = Quaternion.Euler(rotation);
        UIManager.Instance.UpdateCompass(rotationDirection);
        _isMoving = false;
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

    private Vector3 TruncateVector3( Vector3 value )
    {
        value.x = Mathf.RoundToInt( value.x );
        value.y = Mathf.RoundToInt( value.y );
        value.z = Mathf.RoundToInt( value.z );

        return value;
    }

    public void StopAllRoutines()
    {
        StopAllCoroutines();
        _isMoving = false;
    }
}
