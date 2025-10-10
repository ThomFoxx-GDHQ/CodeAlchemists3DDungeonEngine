using UnityEngine;

public class CompassBehavior : MonoBehaviour
{
    [SerializeField] Transform _rose;
    Direction _forwardDirection;
    Vector3 _rotation = Vector3.zero;


    public void SetDirection(Direction forwardDirection)
    {
        _rotation = _rose.rotation.eulerAngles;
        _forwardDirection = forwardDirection;

        switch (forwardDirection)
        {
            case Direction.North:
                _rotation.z = 0;
                break;
            case Direction.East:
                _rotation.z = 90;
                break;
            case Direction.South:
                _rotation.z = 180;
                break;
            case Direction.West:
                _rotation.z = 270;
                break;
            default:
                break;
        }

        _rose.rotation = Quaternion.Euler(_rotation);
    }
}
