using UnityEngine;

public class CompassBehavior : MonoBehaviour
{
    [SerializeField] Transform _rose;
    Directions _forwardDirection;
    Vector3 _rotation = Vector3.zero;


    public void SetDirection(Directions forwardDirection)
    {
        _rotation = _rose.rotation.eulerAngles;
        _forwardDirection = forwardDirection;

        switch (forwardDirection)
        {
            case Directions.North:
                _rotation.z = 0;
                break;
            case Directions.East:
                _rotation.z = 90;
                break;
            case Directions.South:
                _rotation.z = 180;
                break;
            case Directions.West:
                _rotation.z = 270;
                break;
            default:
                break;
        }

        _rose.rotation = Quaternion.Euler(_rotation);
    }
}
