using UnityEngine;

public class StairsBehavior : MonoBehaviour
{
    private enum StairDirection
    {
        Up,
        Down
    }

    [SerializeField] private StairDirection _direction;
    [SerializeField] private int _numberOfFloorsToChange = 1;
    [SerializeField] private bool _active = false;
    private int _floorToGoTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _active)
        { 
            if (_direction == StairDirection.Up)
            {
                _floorToGoTo = DungeonManager.Instance.Floor - _numberOfFloorsToChange;
            }
            else if (_direction == StairDirection.Down)
            {
                _floorToGoTo = DungeonManager.Instance.Floor + _numberOfFloorsToChange;
            }
            
            other.GetComponent<PartyController>().StopAllRoutines();

            DungeonManager.Instance.GenerateNextFloor(_floorToGoTo);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _active = true;
        }
    }
}
